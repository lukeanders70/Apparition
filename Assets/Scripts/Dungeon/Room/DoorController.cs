using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorState state;
    public GameObject insideRoom;
    public GameObject oppostiteDoor;

    [SerializeField]
    private GameObject teleportPoint;

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player" && oppostiteDoor != null)
        {
            Vector2 playerMovePosition = oppostiteDoor.GetComponent<DoorController>().teleportPoint.transform.position;

            collidedObject.GetComponent<PlayerMovement>().ForceTranslate(playerMovePosition, 5);

            GameObject.Find("Main Camera").GetComponent<CameraMoveController>().MoveToObject(oppostiteDoor.GetComponent<DoorController>().insideRoom, 50);
        }
    }
}

public enum DoorState {
    Wall,
    Open,
    Closed,
    Locked
}
