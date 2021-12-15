using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : BasicHealth
{
    [SerializeField]
    private int damage;

    public Rigidbody2D rigidBody;
    public Collider2D collidr;

    public GameObject room;
    public RoomGrid roomGrid;
    public EnemyManager enemyManager;

    virtual protected void Start()
    {
        enemyManager = gameObject.GetComponentInParent<EnemyManager>();
        room = GetComponentInParent<RoomController>().gameObject;
        roomGrid = GetComponentInParent<RoomController>().roomGrid;
        rigidBody = GetComponent<Rigidbody2D>();
        collidr = GetComponent<Collider2D>();
        if (enemyManager == null)
            Debug.LogError("could not find enemy manager for Enemy");
        if (roomGrid == null)
            Debug.LogError("could not find enemy room grid for Enemy");
        if (rigidBody == null)
            Debug.LogError("could not find rigid body for Enemy");
        if (collidr == null)
            Debug.LogError("could not find collider for Enemy");
        if (room == null)
            Debug.LogError("could not find room for Enemy");
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
        base.Kill();
        enemyManager.OnEnemyKilled();
    }
}
