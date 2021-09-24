using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private Vector3 movement;

    [SerializeField]
    CircleCollider2D circleCollider;

    private Vector2 lastPosition;

    private void Start()
    {
        MoveRandomDirection();
    }
    void FixedUpdate()
    {
        if( lastPosition == rb.position)
        {
            forceRaycastBounce();
        }

        rb.velocity = movement * speed;
    }

    void forceRaycastBounce()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            transform.position + (Vector3)circleCollider.offset,
            movement,
            circleCollider.radius + 0.5f
        );
        foreach (RaycastHit2D hit in hits)
        {
            if ((hit.collider != null) && (hit.collider.gameObject != this.gameObject) && (hit.collider.gameObject.tag != "player"))
            {
                Debug.Log(hit.collider.gameObject);
                Debug.Log(hit.point);
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
        } else
        {
            movement = Vector3.Reflect(movement, );
            movement.Normalize();
        }
    }

    void MoveRandomDirection()
    {
        movement = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        movement.Normalize();
    }
}
