using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    private float speed = 10;

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
        }
    }

    public void MoveToObject(GameObject obj, float moveSpeed)
    {
        enabled = true;
        moving = true;
        speed = moveSpeed;
        destination = new Vector3(obj.transform.position.x, obj.transform.position.y, this.transform.position.z);
    }

}
