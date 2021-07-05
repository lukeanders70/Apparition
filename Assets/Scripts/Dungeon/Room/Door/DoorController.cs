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
            PassThrough(collidedObject);
        }
    }

    void PassThrough(GameObject player)
    {
        GameObject oppositeRoom = oppostiteDoor.GetComponent<DoorController>().insideRoom;
        Vector2 playerMovePosition = oppostiteDoor.GetComponent<DoorController>().teleportPoint.transform.position;
        player.GetComponent<PlayerMovement>().ForceTranslate(playerMovePosition, 5);
        GameObject.Find("Main Camera").GetComponent<CameraMoveController>().MoveToObject(
            oppositeRoom,
            50,
            delegate() { 
                insideRoom.GetComponent<RoomController>().ExitRoom();
                oppositeRoom.GetComponent<RoomController>().EnterRoom();
            }
        );
    }
}

public enum DoorState {
    Wall,
    Open,
    Closed,
    Locked
}
