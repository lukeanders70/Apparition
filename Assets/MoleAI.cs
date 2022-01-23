using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleAI : BasicEnemyAI
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    ParticleSystem dive;
    [SerializeField]
    ParticleSystem burrow;
    [SerializeField]
    ParticleSystem surface;

    private AIStateMachine stateMachine;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        stateMachine = new AIStateMachine();
        stateMachine.RegisterState("look", new LookState(gameObject, stateMachine));
        stateMachine.RegisterState("move", new MoveState(gameObject, stateMachine));

        stateMachine.EnterState("look");
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void Stop()
    {
        rigidBody.velocity = Vector2.zero;
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

    private AnimatorStateInfo GetAnimationState()
    {
        return animator.GetCurrentAnimatorStateInfo(0);
    }

    private class LookState : AIState
    {
        private MoleAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Invokable moveInvoker;

        public LookState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<MoleAI>();
            stateMachine = sm;
        }

        override public void Update()
        {
            AIComp.Stop();
            base.Update();
        }


        override public void StartState()
        {
            AIComp.SetAnimationState("idle");
            AIComp.Stop();
            moveInvoker = Invoke(() => { stateMachine.EnterState("move"); }, 3.0f);
        }

        public override void StopState()
        {
            if (moveInvoker != null)
            {
                moveInvoker.Cancel();
            }
            base.StopState();
        }
    }

    private class MoveState : AIState
    {
        private MoleAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Coroutine currentCoroutine;
        private ParticleSystem burrowPs;

        public MoveState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<MoleAI>();
            stateMachine = sm;
        }

        override public void StartState()
        {
            AIComp.Stop();
            var targetPosition = AIHelpers.GetClosestPlayer(AIComp.transform.position).transform.position;
            currentCoroutine = AIComp.StartCoroutine(Dive(targetPosition));
        }

        public override void StopState()
        {
            if (currentCoroutine != null)
            {
                AIComp.StopCoroutine(currentCoroutine);
            }
            if (burrowPs != null)
            {
                burrowPs.Stop();
                Destroy(burrowPs);
            }
            base.StopState();
        }

        private IEnumerator Dive(Vector2 position)
        {
            AIComp.SetAnimationState("move");
            ParticleSystem psInstanceDive = Instantiate(AIComp.dive);
            psInstanceDive.transform.position = AIComp.transform.position;

            while (!AIComp.GetAnimationState().IsName("tunnel"))
            {
                yield return null;
            }

            AIComp.gameObject.layer = 11;
            burrowPs = Instantiate(AIComp.burrow);
            burrowPs.transform.parent = AIComp.gameObject.transform;
            burrowPs.transform.localPosition = new Vector3(0,-0.3f,0);

            currentCoroutine = AIComp.StartCoroutine(AIHelpers.MoveTo(AIComp.rigidBody, position, AIComp.moveSpeed, () => { Surface(); }));
            yield break;
        }

        private void Surface()
        {
            ParticleSystem psInstanceDive = Instantiate(AIComp.surface);
            psInstanceDive.transform.position = AIComp.transform.position;
            stateMachine.EnterState("look");
            AIComp.gameObject.layer = 0;
        }
    }
}
