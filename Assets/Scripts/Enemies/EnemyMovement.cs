using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    private Vector3 movement;

    private void Start()
    {
        MoveRandomDirection();
    }
    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if( collidedObject.tag == "Player")
        {
            Health colliderHealth = collision.collider.GetComponent<Health>();
            if (colliderHealth != null) {
                colliderHealth.Damage(damage);
            }
        }
        else if(collidedObject.tag == "Wall")
        {
            MoveRandomDirection();
        }
    }

    void MoveRandomDirection()
    {
        movement = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        movement.Normalize();
    }
}
