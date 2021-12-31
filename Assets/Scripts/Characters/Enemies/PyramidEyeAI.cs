using System.Collections.Generic;
using UnityEngine;

public class PyramidEyeAI : BasicEnemyAI
{
    public float fireProbability;
    public float speed;
    // Start is called before the first frame update
    private AIStateMachine stateMachine;

    internal Follow targetFollowComp;
    public SpriteRenderer targetSpriteComp;

    public GameObject lazerPrefab;

    override protected void Start()
    {
        base.Start();
        targetFollowComp = GetComponentInChildren<Follow>();
        //targetSpriteComp = GetComponentInChildren<SpriteRenderer>();
        stateMachine = new AIStateMachine();
        stateMachine.RegisterState("walk", new WalkState(gameObject, stateMachine));
        stateMachine.RegisterState("fire", new FireState(gameObject, stateMachine));


        stateMachine.EnterState("walk");
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void Stop()
    {
        rigidBody.velocity = Vector2.zero;
    }

    private class WalkState : AIState
    {
        private PyramidEyeAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Invokable moveInvoker;
        private Invokable changeDirInvoker;
        public WalkState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<PyramidEyeAI>();
            stateMachine = sm;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnCollision(Collision2D collision)
        {
            stateMachine.EnterStateLowPriority("idle");
        }

        override public void StartState()
        {
            AIComp.Stop();
            var newDir = chooseNewDirection();
            if(newDir != null)
            {
                moveInvoker = Invoke(() => { AIComp.rigidBody.velocity = newDir.ToVector2() * AIComp.speed; }, 1.0f);
            }
            changeDirInvoker = Invoke(() => { RandomWalkOrFire(); }, 3.0f);
            base.StartState();
        }

        public override void StopState()
        {
            if (moveInvoker != null) { moveInvoker.Cancel(); }
            if (changeDirInvoker != null) { changeDirInvoker.Cancel(); }
            base.StopState();
        }

        private IntVector2 chooseNewDirection()
        {
            var possible = new List<IntVector2>();
            foreach(IntVector2 direction in AIHelpers.cardinalDirections)
            {
                var testPosition = IntVector2.Add(AIComp.transform.position, direction);
                Collider2D intersects = Physics2D.OverlapBox(
                    testPosition + AIComp.collidr.offset,
                    ((BoxCollider2D)AIComp.collidr).size,
                    0
                );
                if(!intersects)
                {
                    possible.Add(direction);
                }
            }
            if(possible.Count > 0)
            {
                return possible[Random.Range(0, possible.Count)];
            } else
            {
                return null;
            }
        }

        private void RandomWalkOrFire()
        {
            if(Random.value < AIComp.fireProbability)
            {
                stateMachine.EnterState("fire");
            } else
            {
                stateMachine.EnterState("walk");
            }
        }
    }

    private class FireState : AIState
    {
        private PyramidEyeAI AIComp;
        private GameObject gameObject;
        private AIStateMachine stateMachine;

        private Invokable stopFollowInvoker;
        private Invokable fireInvoker;

        public FireState(GameObject go, AIStateMachine sm)
        {
            gameObject = go;
            AIComp = gameObject.GetComponent<PyramidEyeAI>();
            stateMachine = sm;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnCollision(Collision2D collision)
        {
            stateMachine.EnterStateLowPriority("idle");
        }

        override public void StartState()
        {
            AIComp.Stop();
            var inactivePlayer = AIHelpers.GetInactivePlayer();
            Debug.Log("Start Follow");
            StartFollow(inactivePlayer);

            stopFollowInvoker = Invoke(() => {
                Debug.Log("Stop Follow");
                StopFollow();
            }, 4.0f);
            fireInvoker = Invoke(() => {
                Debug.Log("FIRE");
                var lazer = Instantiate(AIComp.lazerPrefab);
                lazer.GetComponent<Lazer>().CreateLazer(AIComp.transform.position, AIComp.targetSpriteComp.transform.position);
                ReturnTarget();
                stateMachine.EnterState("walk");
            }, 5.5f);

            base.StartState();
        }

        public override void StopState()
        {
            if (stopFollowInvoker != null) { stopFollowInvoker.Cancel(); }
            if (fireInvoker != null) { fireInvoker.Cancel(); }
            ReturnTarget();
            base.StopState();
        }

        private void StartFollow(GameObject o)
        {
            AIComp.targetFollowComp.transform.localPosition = Vector2.zero;
            AIComp.targetFollowComp.SetFollow(o);
            AIComp.targetFollowComp.follow = true;
            AIComp.targetSpriteComp.color = Color.white;
        }

        private void StopFollow()
        {
            AIComp.targetFollowComp.SetFollow(null);
            AIComp.targetFollowComp.follow = false;
        }

        private void ReturnTarget()
        {
            StopFollow();
            AIComp.targetSpriteComp.color = Color.clear;
            AIComp.targetFollowComp.transform.localPosition = Vector2.zero;

        }
    }
}
