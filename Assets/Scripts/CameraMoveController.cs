using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class CameraMoveController : MonoBehaviour
{
    private float speed = 10;
    private CameraMoveCallback? moveCallback;

    private Vector3 destination;
    private bool moving = false;

    void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        if (moving)
        {
            if (Vector2.Distance(destination, transform.position) > 0.01)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                return;
            }
        }
        EndMoveToObject();
    }
    private void EndMoveToObject()
    {
        if (moving)
        {
            enabled = false;
            moving = false;
            speed = 0;
            transform.position = new Vector3(destination.x, destination.y, transform.position.z);
            if(moveCallback != null)
            {
                moveCallback();
            }
            moveCallback = null;
        }
    }

    public void MoveToObject(GameObject obj, float moveSpeed, CameraMoveCallback? callback)
    {
        enabled = true;
        moving = true;
        speed = moveSpeed;
        destination = new Vector3(obj.transform.position.x, obj.transform.position.y, this.transform.position.z);
        moveCallback = callback;
    }

}

public delegate void CameraMoveCallback();

