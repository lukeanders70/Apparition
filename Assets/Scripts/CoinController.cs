using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem ps;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collidedObject = collider.gameObject;
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
