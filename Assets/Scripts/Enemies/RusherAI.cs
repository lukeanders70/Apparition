using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RusherAI : BasicEnemyAI
{
    public Rigidbody2D rb;

    private Vector2? intendedLocation;
    [SerializeField] 
    private float speed;
    [SerializeField]
    private Animator animator;

    private Coroutine currentCoroutine = null;

    // Start is called before the first frame update
    override protected void Start()
    {
        SetIntention(null);
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsStopped() && intendedLocation != null)
        {
            if (Vector2.Distance(transform.position, (Vector2)intendedLocation) < 0.05)
            {
                Stop();
            } else
            {
                animator.SetBool("isWalking", true);
                Vector3 dir = ((Vector2)intendedLocation - (Vector2)transform.position).normalized;
                rb.velocity = (dir * speed);
                animator.SetFloat("lastHorizontal", rb.velocity.x);
            }
        } else
        {
            rb.velocity = Vector2.zero;
        }
    }

    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        Stop();
        base.OnCollisionEnter2D(collision);
    }

    private void Stop()
    {
        animator.SetBool("isWalking", false);
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        rb.velocity = Vector2.zero;
        intendedLocation = null;
        currentCoroutine = StartCoroutine(Wander());
    }

    IEnumerator Aggro()
    {
        GameObject closestPlayer = AIHelpers.GetClosestPlayer(transform.position);
        if(closestPlayer == null)
        {
            Stop();
            yield break;
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
        Vector3 closestPlayerPosition = closestPlayer.transform.position;
        yield return new WaitForSeconds(0.5f);
        SetIntention(closestPlayerPosition);
    }

    private bool IsStopped()
    {
        return (intendedLocation == null) && (rb.velocity == Vector2.zero);
    }


    IEnumerator Wander()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(3);
        SetIntention(null);
    }

    private void SetIntention(Vector2? position)
    {
        if(position != null)
        {
            intendedLocation = position;
        } else
        {
            var boxCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
            foreach (int _ in Enumerable.Range(1, 3))
            {
                Vector3 dir = AIHelpers.RandomDirection();
                var intendedDistance = Random.Range(1.0f, 3.0f);
                var testPosition = transform.position + (dir * intendedDistance);
                Collider2D intersects = Physics2D.OverlapBox(
                    (Vector2)testPosition + boxCollider.offset,
                    boxCollider.size,
                    0
                );
                if(intersects == null)
                {
                    intendedLocation = testPosition;
                    break;
                } else
                {
                    Stop();
                }
            }
        }

    }

    override public bool Damage(int damage)
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Aggro());
        return base.Damage(damage);
    }
}
