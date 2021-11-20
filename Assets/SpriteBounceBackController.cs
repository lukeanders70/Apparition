using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBounceBackController : MonoBehaviour
{
    public SpiritController spiritController;

    private bool onReturnTrip = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.GetComponent<Collider2D>().gameObject;
        if (spiritController.isMoving && collidedObject.tag == "Wall" && !onReturnTrip)
        {
            onReturnTrip = true;
            ParticleSystem hitParticals = Instantiate(spiritController.ps);
            hitParticals.transform.position = transform.position;
            spiritController.StartExclusiveMove(spiritController.lastParent);
        } else if (collidedObject.tag == "Player")
        {
            onReturnTrip = false;
        }
    }
}
