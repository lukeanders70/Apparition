using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject doorwayWallPrefab;
    [SerializeField]
    private GameObject doorPrefab;
    
    #nullable enable
    public GameObject AddDoor(GameObject room, GameObject? oppostiteDoor)
    {
        GameObject door = GameObject.Instantiate(doorPrefab, transform.position, transform.rotation);
        door.GetComponent<DoorController>().insideRoom = room;
        door.GetComponent<DoorController>().oppostiteDoor = oppostiteDoor;
        door.transform.parent = room.transform;

        if(oppostiteDoor != null)
        {
            oppostiteDoor.GetComponent<DoorController>().oppostiteDoor = door;
        }

        return door;
    }

    public GameObject AddDoorwayWall(GameObject room)
    {
        GameObject doorwayWall = GameObject.Instantiate(doorwayWallPrefab, transform.position, transform.rotation);
        doorwayWall.transform.parent = room.transform;
        return doorwayWall;
    }
}
