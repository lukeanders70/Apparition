using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject doorwayWallPrefab;
    [SerializeField]
    private GameObject doorPrefab;
    [SerializeField]
    private string direction;
    
    #nullable enable
    public GameObject AddDoor(GameObject room, GameObject? oppostiteDoor)
    {
        GameObject door = Instantiate(doorPrefab, transform.position, transform.rotation);
        door.GetComponent<DoorController>().insideRoom = room;
        door.GetComponent<DoorController>().oppostiteDoor = oppostiteDoor;
        door.transform.parent = room.transform;

        if(oppostiteDoor != null)
        {
            oppostiteDoor.GetComponent<DoorController>().oppostiteDoor = door;
        }

        return door;
    }

    public GameObject AddDoorwayWall(GameObject room, string wallType)
    {
        var path = "images/Room/" + wallType + "/" + direction + "doorwayWall";
        Sprite doorwayWallSprite = Resources.Load<Sprite>(path);
        GameObject doorwayWall = Instantiate(doorwayWallPrefab, transform.position, transform.rotation);
        doorwayWall.transform.parent = room.transform;
        if (doorwayWallSprite != null)
        {
            doorwayWall.GetComponent<SpriteRenderer>().sprite = doorwayWallSprite;
        } else
        {
            Debug.LogError("Could not find doorway wall at " + path);
        }
        return doorwayWall;
    }
}
