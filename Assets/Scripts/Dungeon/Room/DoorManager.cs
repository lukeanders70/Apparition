using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorManager : MonoBehaviour
{
    public float doorSpawnProbablity;

    [SerializeField]
    private GameObject leftDoorPosition;
    [SerializeField]
    private GameObject rightDoorPosition;
    [SerializeField]
    private GameObject bottomDoorPosition;
    [SerializeField]
    private GameObject topDoorPosition;

    private Dictionary<Vector2, GameObject> doorPositions;

    public Dictionary<Vector2, GameObject> doors = new Dictionary<Vector2, GameObject>();

    public void Awake()
    {
        doorPositions = new Dictionary<Vector2, GameObject>()
        {
            { Constants.directions["up"], topDoorPosition },
            { Constants.directions["down"], bottomDoorPosition },
            { Constants.directions["left"], leftDoorPosition },
            { Constants.directions["right"], rightDoorPosition }
        };
    }

    public void Start()
    {
        var wallType = gameObject.GetComponent<RoomController>().roomInfo.WallType;
        PlugInvalidDoors(wallType);
    }

    private void PlugInvalidDoors(string wallType)
    {
        foreach (Vector2 direction in Constants.directions.Values)
        {
            GameObject door = doors[direction];
            if (door != null && !door.GetComponent<DoorController>().Navigable())
            {
                AddDoorwayWall(direction, wallType);
            }
        }
    }

    public void SetupDoors(Dictionary<Vector2, GameObject> dungeon, Vector2 position, string wallType, float? doorSpawnProbabilityOverride)
    {
        foreach (Vector2 direction in Constants.directions.Values)
        {
            Vector2 oppositeDirection = direction * -1;
            Vector2 neighboorPosition = position + direction;
            if(dungeon.ContainsKey(neighboorPosition))
            {
                GameObject neighbooringRoom = dungeon[neighboorPosition];
                GameObject oppositeRoomDoor = neighbooringRoom.GetComponent<DoorManager>().doors[oppositeDirection];
                if (oppositeRoomDoor.GetComponent<DoorController>().state == DoorState.Open)
                {
                    AddDoor(direction, oppositeRoomDoor);
                } else
                {
                    AddDoorwayWall(direction, wallType);
                }
            } else if (doorSpawnProbabilityOverride == null ? Random.value < doorSpawnProbablity : Random.value < doorSpawnProbabilityOverride)
            {
                AddDoor(direction, null);
            } else
            {
                AddDoorwayWall(direction, wallType);
            }
        }
    }

    #nullable enable
    public void AddDoor(Vector2 direction, GameObject? oppositeRoomDoor)
    {
        GameObject newDoor = doorPositions[direction].GetComponent<DoorwayGenerator>().AddDoor(gameObject, oppositeRoomDoor);
        if (doors.ContainsKey(direction))
        {
            Destroy(doors[direction]);
            doors.Remove(direction);
        }
        doors.Add(direction, newDoor);
    }
    public void AddDoorwayWall(Vector2 direction, string wallType)
    {
        GameObject newDoor = doorPositions[direction].GetComponent<DoorwayGenerator>().AddDoorwayWall(gameObject, wallType);
        if (doors.ContainsKey(direction))
        {
            Destroy(doors[direction]);
            doors.Remove(direction);
        }
        doors.Add(direction, newDoor);
    }

    public void ResetDoorwayWallSprites(string wallType)
    {
        var directions = doors.Keys.ToList();
        foreach (Vector2 doorDirection in directions)
        {
            GameObject door = doors[doorDirection];
            if (door.GetComponent<DoorController>().state == DoorState.Wall)
            {
                AddDoorwayWall(doorDirection, wallType);
            }
        }
    }

    public List<Vector2> getAccessibleDirections()
    {
        List<Vector2> accessiblePositions = new List<Vector2>();
        foreach(Vector2 doorDirection in doors.Keys)
        {
            GameObject door = doors[doorDirection];
            if(door.GetComponent<DoorController>().state == DoorState.Open)
            {
                accessiblePositions.Add(doorDirection);
            }
        }
        return accessiblePositions;
    }
}
