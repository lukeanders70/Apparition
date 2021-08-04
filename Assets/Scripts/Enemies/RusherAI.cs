using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class RusherAI : BasicHealth
{
    public Rigidbody2D rb;

    private Vector2? intendedLocation;
    [SerializeField] 
    private float speed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private Animator animator;

    private Coroutine? currentCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        SetIntention(null);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player")
        {
            Health colliderHealth = collision.collider.GetComponent<Health>();
            if (colliderHealth != null)
            {
                colliderHealth.Damage(damage);
            }
        }
        else
        {
            Vector3 oppositeDirection = collision.contacts[0].normal;
            oppositeDirection.Normalize();
            Stop();
            SetIntention(transform.position + (oppositeDirection * 0.3f));
        }
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
        GameObject? closestPlayer = AIHelpers.GetClosestPlayer(transform.position);
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
            Vector3 dir = AIHelpers.RandomDirection();
            var intendedDistance = Random.Range(1.0f, 3.0f);
            intendedLocation = transform.position + (dir * intendedDistance);
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
