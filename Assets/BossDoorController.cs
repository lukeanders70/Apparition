using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorController : DoorController
{
    [SerializeField]
    public Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player")
        {
            InventoryController inventory = collidedObject.GetComponentInParent<InventoryController>();
            if (inventory.RemoveKey()) {
                SetOpen();
            }
        }
    }

    private void SetClosed()
    {
        state = DoorState.Closed;
        animator.SetBool("doorOpen", false);
    }

    private void SetOpen()
    {
        state = DoorState.Open;
        animator.SetBool("doorOpen", true);
    }
}
