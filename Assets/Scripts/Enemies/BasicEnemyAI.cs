using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : BasicHealth
{
    [SerializeField]
    private int damage;

    virtual public void OnCollisionEnter2D(Collision2D collision)
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
    }
}
