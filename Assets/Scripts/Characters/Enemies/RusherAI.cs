using System.Collections.Generic;
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

    private AIStateMachine stateMachine;

    private PathFinder pathFinder;

    public HashSet<IntVector2> visited = new HashSet<IntVector2> { };
    public Dictionary<IntVector2, (float H, PathNode node)> fringe = new Dictionary<IntVector2, (float H, PathNode node)> { };
    public IntVector2 active = new IntVector2(0, 0);

    override protected void Start()
    {
        base.Start();

        pathFinder = GetComponentInParent<PathFinder>();
        if (pathFinder == null)
            Debug.LogError("could not find pathfinder for rusher enemy");

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
        if(base.Damage(damage))
        {
            stateMachine.EnterState("aggro");
            return true;
        }
        return false;
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
            stateMachine.EnterStateLowPriority("idle");
        }

        override public void StartState()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            AIComp.SetAnimationState("idle");
            AIComp.Stop();
            walkInvoker = Invoke(() => { stateMachine.EnterState("walk"); }, 3.0f);
        }

        public override void StopState()
        {
            if (walkInvoker != null)
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
            stateMachine.EnterStateLowPriority("idle");
        }

        override public void StartState()
        {
            AIComp.SetAnimationState("walk");
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            AIComp.Stop();
            var walkPoint = AIComp.getWanderPoint();
            var spline = AIComp.pathFinder.Pathfind(AIComp.room, gameObject, walkPoint);
            walkRoutine = AIComp.StartCoroutine(AIComp.pathFinder.MoveAlongSpline(AIComp.rigidBody, AIComp.room.transform.position, spline, AIComp.walkSpeed, () =>
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

        public AggroState(GameObject go, AIStateMachine sm)
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
            if(collision.gameObject.tag == "wall")
            {
                stateMachine.EnterStateLowPriority("idle");
            }
        }

        override public void StartState()
        {
            Notice();
            base.StartState();
        }

        public override void StopState()
        {
            AIComp.invicible = false;
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
            Invoke(() => { AIComp.invicible = true; }, 0.1f);
            AIComp.SetAnimationState("idle");
            AIComp.Stop();
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
            var runPoint = AIHelpers.GetClosestPlayer(gameObject.transform.position).transform.position;

            runInvoke = Invoke(() => Run(runPoint), 1.0f);
        }

        private void Run(Vector2 runPoint)
        {
            AIComp.SetAnimationState("run");
            var spline = AIComp.pathFinder.Pathfind(AIComp.room, gameObject, runPoint);
            runRoutine = AIComp.StartCoroutine(AIComp.pathFinder.MoveAlongSpline(AIComp.rigidBody, AIComp.room.transform.position,  spline, AIComp.runSpeed, () =>
            {
                stateMachine.EnterState("idle");
            }));
        }
    }
}
