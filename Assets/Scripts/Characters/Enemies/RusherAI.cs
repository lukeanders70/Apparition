using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RusherAI : BasicEnemyAI
{
    public Rigidbody2D rb;

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
    private float aggroRange;
    [SerializeField]
    private int numCharges;

    private string state;
    private int numChargesRemaining = 0;

    private Coroutine currentCoroutine = null;

    // Start is called before the first frame update
    override protected void Start()
    {
        StartCoroutine(setStopped());
        base.Start();
    }

    IEnumerator setStopped()
    {
        state = "idle";

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        Stop();
        yield return new WaitForSeconds(3);
        conditionallyStopCoroutine();
        currentCoroutine = StartCoroutine(setWalk());
        yield break;
    }

    IEnumerator setAggro(bool resetCharges = true)
    {
        state = "aggro";

        if (resetCharges)
        {
            numChargesRemaining = numCharges;
        }

        Stop();
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
        var runPoint = AIHelpers.GetClosestPlayer(transform.position).transform.position;
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("run", true);
        animator.SetBool("idle", false);
        animator.SetBool("walk", false);
        while (Vector2.Distance(runPoint, transform.position) > 0.15)
        {
            rb.velocity = getVelocityTowardsPoint(runPoint, runSpeed);
            animator.SetFloat("lastHorizontal", rb.velocity.x);
            yield return null;
        }
        numChargesRemaining = numChargesRemaining - 1;
        conditionallyStopCoroutine();
        if (numChargesRemaining <= 0)
        {
            numChargesRemaining = numCharges;
            conditionallyStopCoroutine();
            currentCoroutine = StartCoroutine(setStopped());
        }
        else
        {
            conditionallyStopCoroutine();
            currentCoroutine = StartCoroutine(setAggro(false));
        }
        yield break;
    }

    IEnumerator setWalk()
    {
        state = "walk";

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        var walkPoint = getWanderPoint();
        animator.SetBool("run", false);
        animator.SetBool("idle", false);
        animator.SetBool("walk", true);
        var velocity = getVelocityTowardsPoint(walkPoint, walkSpeed);
        while (Vector2.Distance(walkPoint, (Vector2)transform.position) > 0.15)
        {
            rb.velocity = velocity;
            animator.SetFloat("lastHorizontal", rb.velocity.x);
            yield return null;
        }
        conditionallyStopCoroutine();
        currentCoroutine = StartCoroutine(setStopped());
        yield break;
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

    private void conditionallyStopCoroutine() {
        if (currentCoroutine != null) { StopCoroutine(currentCoroutine); } 
    }

    private void Stop()
    {
        animator.SetBool("run", false);
        animator.SetBool("idle", true);
        animator.SetBool("walk", false);
        rb.velocity = Vector2.zero;
    }


    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        conditionallyStopCoroutine();
        if (state != "aggro")
        {
            currentCoroutine = StartCoroutine(setStopped());
        } else
        {
            numChargesRemaining = numChargesRemaining - 1;
            currentCoroutine = StartCoroutine(setAggro(false));
        }
        base.OnCollisionEnter2D(collision);
    }

    override public bool Damage(int damage)
    {
        if(currentCoroutine != null && state != "aggro")
        {
            conditionallyStopCoroutine();
            currentCoroutine = StartCoroutine(setAggro());
        }
        return base.Damage(damage);
    }
}
