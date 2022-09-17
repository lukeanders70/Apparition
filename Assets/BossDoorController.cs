using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorController : DoorController
{
    [SerializeField]
    public Animator animator;

    private void Start()
    {
        state = DoorState.Locked;
    }

    virtual protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (state == DoorState.Locked)
        {
            if (collidedObject.tag == "Player")
            {
                InventoryController inventory = collidedObject.GetComponentInParent<InventoryController>();
                if (inventory.RemoveKey())
                {
                    SetOpen();
                }
            }
        }
        else
        {
            if (collidedObject.tag == "Player" && oppostiteDoor != null)
            {
                PassThrough(collidedObject);
            }
        }
    }

    private void SetClosed()
    {
        state = DoorState.Locked;
        animator.SetBool("doorOpen", false);
    }

    private void SetOpen()
    {
        state = DoorState.Open;
        animator.SetBool("doorOpen", true);
    }
}
