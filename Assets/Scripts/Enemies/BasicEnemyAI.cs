using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : BasicHealth
{
    [SerializeField]
    private int damage;

    private EnemyManager enemyManager;

    virtual protected void Start()
    {
        Debug.Log("starting");
        enemyManager = gameObject.GetComponentInParent<EnemyManager>();
        if(enemyManager == null)
            Debug.LogError("could not find enemy manager for Enemy");
    }

    virtual protected void OnCollisionEnter2D(Collision2D collision)
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

    override public void Kill()
    {
        Debug.Log("Calling Enemy Manager Killed");
        base.Kill();
        enemyManager.OnEnemyKilled();
    }
}
