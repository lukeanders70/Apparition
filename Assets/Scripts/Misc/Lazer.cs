using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem ps;
    public BoxCollider2D boxCollider;
    public void CreateLazer(Vector2 start, Vector2 end)
    {
        var length = Vector2.Distance(start, end);
        var position = (start + end) * 0.5f;

        var vectorInDirection = (end - start);
        var rotation = Mathf.Atan2(vectorInDirection.y, vectorInDirection.x);

        var shape = ps.shape;
        shape.radius = length / 2;
        boxCollider.size = new Vector2(length, boxCollider.size.y);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * rotation));
        //shape.rotation = new Vector3(0, -Mathf.Rad2Deg * rotation, 0);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.tag == "Player")
        {
            Health colliderHealth = collision.GetComponent<Health>();
            if (colliderHealth != null)
            {
                colliderHealth.Damage(1);
            }
        }
    }
}
