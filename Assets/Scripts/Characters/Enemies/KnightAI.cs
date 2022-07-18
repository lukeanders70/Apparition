using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI : BasicEnemyAI
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int minWanderDistance;
    [SerializeField]
    private int maxWanderDistance;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private ShieldController shield;
    [SerializeField]
    private ShieldController shield2;
    [SerializeField]
    private SwordController sword;
    [SerializeField]
    private SwipeController attackEffect;

    private SwipeController currentEffect = null;

    private Vector2 lastHitDirection;

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
            Debug.LogError("could not find pathfinder for knight enemy");

        stateMachine = new AIStateMachine();
        stateMachine.RegisterState("idle", new IdleState(gameObject, stateMachine));
        stateMachine.RegisterState("aggro", new AggroState(gameObject, stateMachine));
        stateMachine.RegisterState("walk", new WalkState(gameObject, stateMachine));
        stateMachine.RegisterState("stagger", new StaggerState(gameObject, stateMachine));

        stateMachine.EnterState("idle");
    }

    public void ShieldHit()
    {
        invicible = true;
        shield.Hit();
        shield2.Hit();
        Invoke("ShieldHitOver", 0.5f);
    }
    private void ShieldHitOver()
    {
        shield.HitOver();
        shield2.HitOver();
        invicible = false;
    }

    public void Update()
    {
        stateMachine.Update();
    }

    public void Attack(Vector2 direction)
    {
        sword.Attack();
    }

    private void SetAnimationState(string state)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == state)
            {
               animator.SetBool(param.name, true);
            }
            else if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
        }
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var spriritController = collider.gameObject.GetComponent<SpiritController>();
        if (spriritController != null)
        {
            lastHitDirection = spriritController.direction;
        }
    }

    override public bool Damage(int damage)
    {
        if (base.Damage(damage))
        {
            shield.PauseShield();
            shield2.PauseShield();
            stateMachine.EnterState("stagger");
            return true;
        }
        return false;
    }

    private class StaggerState : AIState
    {
        private KnightAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;
        private Coroutine staggerRoutine;

        public StaggerState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<KnightAI>();
            stateMachine = sm;
        }

        override public void StartState()
        {
            AIComp.shield.PauseShield();
            AIComp.shield2.PauseShield();
            AIComp.Stop();
            AIComp.rigidBody.AddForce(AIComp.lastHitDirection * 1);
            AIComp.spriteRenderer.color = new Color(1, 1, 1, 1);
            AIComp.SetAnimationState("idle");

            staggerRoutine = AIComp.StartCoroutine(AIHelpers.MoveTo(
                AIComp.rigidBody,
                AIComp.transform.position + (Vector3)(AIComp.lastHitDirection * 0.35f),
                0.8f,
                () => stateMachine.EnterState("aggro")
            ));
        }

        public override void StopState()
        {
            AIComp.shield.UnPauseSheild();
            AIComp.shield2.UnPauseSheild();
            if (staggerRoutine != null)
            {
                AIComp.StopCoroutine(staggerRoutine);
            }
            base.StopState();
        }
    }

    private class IdleState : AIState
    {
        private KnightAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Invokable walkInvoker;

        public IdleState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<KnightAI>();
            stateMachine = sm;
        }

        public override void OnCollision(Collision2D collision)
        {
            stateMachine.EnterStateLowPriority("idle");
        }

        override public void StartState()
        {
            AIComp.spriteRenderer.color = new Color(1, 1, 1, 1);
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
        private KnightAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Coroutine walkRoutine;

        public WalkState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<KnightAI>();
            stateMachine = sm;
        }

        public override void Update()
        {
            AIComp.animator.SetFloat("LastHorizontal", AIComp.rigidBody.velocity.x);
            AIComp.animator.SetFloat("LastVertical", AIComp.rigidBody.velocity.y);
            if(AIComp.rigidBody.velocity.x > 0)
            {
                AIComp.sword.SetRight();
            } else
            {
                AIComp.sword.SetLeft();
            }
            base.Update();
        }

        public override void OnCollision(Collision2D collision)
        {
            stateMachine.EnterStateLowPriority("idle");
        }

        override public void StartState()
        {
            AIComp.SetAnimationState("walk");
            AIComp.Stop();
            var spline = AIComp.pathFinder.WanderFind(gameObject, AIComp.minWanderDistance, AIComp.maxWanderDistance);
            walkRoutine = AIComp.StartCoroutine(AIComp.pathFinder.MoveAlongSpline(AIComp.rigidBody, AIComp.room.transform.position, spline, AIComp.walkSpeed, () =>
            {
                stateMachine.EnterState("idle");
            }));
            base.StartState();
        }

        public override void StopState()
        {
            if (walkRoutine != null)
            {
                AIComp.StopCoroutine(walkRoutine);
            }
            AIComp.Stop();
            base.StopState();
        }
    }

    private class AggroState : AIState
    {
        private KnightAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Coroutine runRoutine;
        private Invokable runInvoke;

        public AggroState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<KnightAI>();
            stateMachine = sm;
        }

        public override void Update()
        {
            AIComp.animator.SetFloat("LastHorizontal", AIComp.rigidBody.velocity.x);
            AIComp.animator.SetFloat("LastVertical", AIComp.rigidBody.velocity.x);
            if (AIComp.rigidBody.velocity.x > 0)
            {
                AIComp.sword.SetRight();
            } else
            {
                AIComp.sword.SetLeft();
            }
            if (AIComp.currentEffect == null) {
                var closestPlayerDistance = AIHelpers.ClosestPlayerDistance(gameObject.transform.position);
                if(closestPlayerDistance < 2){
                    var attackDirection = AIHelpers.GetClosestPlayerDirection(gameObject.transform.position);
                    if(attackDirection != null)
                    {
                        AIComp.Attack((Vector2)attackDirection);
                    } else
                    {
                        Debug.Log("Knight tried to attack, but failed to find direction of suitable target");
                    }
                }
            }
            base.Update();
        }

        public override void OnCollision(Collision2D collision)
        {

            stateMachine.EnterStateLowPriority("idle");
        }

        override public void StartState()
        {
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
            var closestPlayer = AIHelpers.GetClosestPlayer(gameObject.transform.position);
            if(closestPlayer != null)
            {
                var walkPoint = AIHelpers.GetClosestEmpty(new List<Vector3> { new Vector3(1.0f, 0.0f), new Vector3(-1.0f, 0.0f) }, closestPlayer.transform.position);
                if(walkPoint != null)
                {
                    runInvoke = Invoke(() => Walk((Vector2)walkPoint), 0.5f);
                    return;
                } else
                {
                    Debug.Log("Failed To File Walk Point");
                }
            }
            stateMachine.EnterState("idle");
        }

        private void Walk(Vector2 walkPoint)
        {
            AIComp.SetAnimationState("walk");
            var spline = AIComp.pathFinder.Pathfind(AIComp.room, gameObject, walkPoint);
            runRoutine = AIComp.StartCoroutine(AIComp.pathFinder.MoveAlongSpline(AIComp.rigidBody, AIComp.room.transform.position, spline, AIComp.walkSpeed, () =>
            {
                stateMachine.EnterState("idle");
            }));
        }
    }
}
