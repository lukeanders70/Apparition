using System.Linq;
using UnityEngine;

public class RusherAI : BasicEnemyAI
{
    [SerializeField] 
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float minWanderDistance;
    [SerializeField]
    private float maxWanderDistance;
    [SerializeField]
    private int numCharges;

    private AIStateMachine stateMachine;

    override protected void Start()
    {
        base.Start();

        stateMachine = new AIStateMachine();
        stateMachine.RegisterState("idle", new IdleState(gameObject, stateMachine));
        stateMachine.RegisterState("aggro", new AggroState(gameObject, stateMachine));
        stateMachine.RegisterState("walk", new WalkState(gameObject, stateMachine));

        stateMachine.EnterState("idle");
    }

    public void Update()
    {
        stateMachine.Update();
    }

    private void SetAnimationState(string state)
    {
        foreach(AnimatorControllerParameter param in animator.parameters)
        {
            if(param.name == state)
            {
                animator.SetBool(param.name, true);
            } else if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
        }
    }

    Vector2 getWanderPoint()
    {
        var boxCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
        foreach (int _ in Enumerable.Range(1, 5))
        {
            Vector3 dir = AIHelpers.RandomDirection();
            var intendedDistance = Random.Range(minWanderDistance, maxWanderDistance);
            var testPosition = transform.position + (dir * intendedDistance);
            Collider2D intersects = Physics2D.OverlapBox(
                (Vector2)testPosition + boxCollider.offset,
                boxCollider.size,
                0
            );
            if (intersects == null)
            {
                return testPosition;
            }
        }
        return transform.position;
    }

    Vector2 getVelocityTowardsPoint(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        return dir * speed;
    }

    private void Stop()
    {
        rigidBody.velocity = Vector2.zero;
    }

    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        stateMachine.OnCollision(collision);
        base.OnCollisionEnter2D(collision);
    }

    override public bool Damage(int damage)
    {
        stateMachine.EnterState("aggro");
        return base.Damage(damage);
    }



    private class IdleState : AIState
    {
        private RusherAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Invokable walkInvoker;

        public IdleState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<RusherAI>();
            stateMachine = sm;
        }

        public override void OnCollision(Collision2D collision)
        {
            stateMachine.EnterState("idle");
        }

        override public void StartState()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            AIComp.SetAnimationState("idle");
            AIComp.Stop();
            walkInvoker = Invoke(() => stateMachine.EnterState("walk"), 3.0f);
        }

        public override void StopState()
        {
            if(walkInvoker != null)
            {
                walkInvoker.Cancel();
            }
            base.StopState();
        }
    }

    private class WalkState : AIState
    {
        private RusherAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Coroutine walkRoutine;

        public WalkState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<RusherAI>();
            stateMachine = sm;
        }

        public override void Update()
        {
            AIComp.animator.SetFloat("lastHorizontal", AIComp.rigidBody.velocity.x);
            base.Update();
        }

        public override void OnCollision(Collision2D collision)
        {
            stateMachine.EnterState("idle");
        }

        override public void StartState()
        {
            AIComp.SetAnimationState("walk");
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            AIComp.Stop();
            var walkPoint = AIComp.getWanderPoint();
            var spline = AIHelpers.Pathfind(AIComp.room, gameObject, walkPoint);
            walkRoutine = AIComp.StartCoroutine(AIHelpers.MoveAlongSpline(AIComp.rigidBody, AIComp.collider.offset, AIComp.room.transform.position, spline, AIComp.walkSpeed, () =>
            {
                stateMachine.EnterState("idle");
            }));
            base.StartState();
        }

        public override void StopState()
        {
            if(walkRoutine != null)
            {
                AIComp.StopCoroutine(walkRoutine);
            }
            AIComp.Stop(); 
            base.StopState();
        }
    }

    private class AggroState : AIState
    {
        private RusherAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Coroutine runRoutine;
        private Invokable runInvoke;

        private int numChargesLeft;

        public AggroState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<RusherAI>();
            stateMachine = sm;

            numChargesLeft = AIComp.numCharges;
        }

        public override void Update()
        {
            AIComp.animator.SetFloat("lastHorizontal", AIComp.rigidBody.velocity.x);
            base.Update();
        }

        public override void OnCollision(Collision2D collision)
        {
            AIComp.Stop();
            if (numChargesLeft <= 0)
            {
                stateMachine.EnterState("idle");
            } else
            {
                numChargesLeft -= 1;
                Notice();
            }
        }

        override public void StartState()
        {
            numChargesLeft = AIComp.numCharges;
            Notice();
            base.StartState();
        }

        public override void StopState()
        {
            if (runRoutine != null)
            {
                AIComp.StopCoroutine(runRoutine);
            }
            if (runInvoke != null)
            {
                runInvoke.Cancel();
            }
            AIComp.Stop();
            base.StopState();
        }

        private void Notice()
        {
            AIComp.SetAnimationState("idle");
            AIComp.Stop();
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
            var runPoint = AIHelpers.GetClosestPlayer(gameObject.transform.position).transform.position;

            runInvoke = Invoke(() => Run(runPoint), 1.0f);
        }

        private void Run(Vector2 runPoint)
        {
            AIComp.SetAnimationState("run");
            var spline = AIHelpers.Pathfind(AIComp.room, gameObject, runPoint);
            runRoutine = AIComp.StartCoroutine(AIHelpers.MoveAlongSpline(AIComp.rigidBody, AIComp.collider.offset, AIComp.room.transform.position,  spline, AIComp.runSpeed, () =>
            {
                stateMachine.EnterState("idle");
            }));
        }
    }
}
