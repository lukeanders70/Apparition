using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private Vector2 movement;

    [SerializeField]
    CircleCollider2D circleCollider;

    private void Start()
    {
        MoveRandomDirection();
    }
    void FixedUpdate()
    {
        if (IsSliding())
        {
            ForceRaycastBounce();
        }

        rb.velocity = movement * speed;
    }

    private bool IsSliding()
    {
        return rb.velocity.magnitude < (speed * 0.9);
    }

    private void ForceRaycastBounce()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            (Vector2)transform.position + circleCollider.offset,
            movement,
            circleCollider.radius + 1.0f
        );
        foreach (RaycastHit2D hit in hits)
        {
            if ((hit.collider != null) && (hit.collider.gameObject != this.gameObject) && (hit.collider.gameObject.tag != "player"))
            {
                movement = Vector3.Reflect(movement, hit.normal);
                movement.Normalize();
                break;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if( collidedObject.tag == "Player")
        {
            Health colliderHealth = collision.collider.GetComponent<Health>();
            if (colliderHealth != null) {
                colliderHealth.Damage(damage);
            }
        }
        else
        {
            movement = Vector2.Reflect(movement, collision.GetContact(0).normal);
            movement.Normalize();
        }
    }

    void MoveRandomDirection()
    {
        movement = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        movement.Normalize();
    }
}
