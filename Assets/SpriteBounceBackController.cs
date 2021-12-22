using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBounceBackController : MonoBehaviour
{
    public SpiritController spiritController;
    public SpriteRenderer spriteRenderer;

    private bool onReturnTrip = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.GetComponent<Collider2D>().gameObject;
        if (ShouldReturn(collidedObject))
        {

            onReturnTrip = true;
            ParticleSystem hitParticals = Instantiate(spiritController.hitPs);
            hitParticals.transform.position = transform.position;
            spiritController.StartExclusiveMove(spiritController.lastParent);
        } else if (collidedObject.tag == "Player")
        {
            onReturnTrip = false;
        }
    }

    private bool ShouldReturn(GameObject collidedObject)
    {
        return (spiritController.isMoving && !onReturnTrip) && ( // available for return
                (collidedObject.tag == "Wall" && collidedObject.layer >= 8) || // is solid wall
                (collidedObject.GetComponent<BasicHealth>() != null && collidedObject.GetComponent<BasicHealth>().invicible == true) // is invincible enemy
            );
    }
}
