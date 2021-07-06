using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorState state;
    public GameObject insideRoom;
    public GameObject oppostiteDoor;

    [SerializeField]
    private GameObject teleportPoint1;
    [SerializeField]
    private GameObject teleportPoint2;

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player" && oppostiteDoor != null)
        {
            PassThrough(collidedObject);
        }
    }

    void PassThrough(GameObject activePlayer)
    {
        GameObject inactivePlayer = activePlayer.transform.parent.GetComponent<PlayerHandler>().getOtherPlayer(activePlayer);

        GameObject oppositeRoom = oppostiteDoor.GetComponent<DoorController>().insideRoom;
        Vector2 activePlayerMovePosition = oppostiteDoor.GetComponent<DoorController>().teleportPoint1.transform.position;
        Vector2 inactivePlayerMovePosition = oppostiteDoor.GetComponent<DoorController>().teleportPoint2.transform.position;

        activePlayer.GetComponent<PlayerMovement>().ForceTranslate(
            activePlayerMovePosition,
            5,
            delegate()
            {
                inactivePlayer.GetComponent<PlayerMovement>().teleportTranslate(inactivePlayerMovePosition);
            }
        );

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
