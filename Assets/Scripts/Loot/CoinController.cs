using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : BasicLootController
{

    [SerializeField]
    private ParticleSystem ps;

    virtual protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player")
        {
            InventoryController inventory = collidedObject.GetComponentInParent<InventoryController>();
            inventory.AddCoins(1);
            ParticleSystem hitParticals = Instantiate(ps);
            hitParticals.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
