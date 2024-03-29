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

    public bool doorsClosed = false;


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
            if (door != null && !door.GetComponent<DoorController>().IsRealDoor())
            {
                AddDoorwayWall(direction, wallType);
            }
        }
    }

    public void SetupDoors(
        Dictionary<Vector2, GameObject> dungeon,
        Vector2 position,
        StaticDungeon.Room roomInfo,
        float? doorSpawnProbabilityOverride
    )
    {
        var isLockedIn = Random.value < roomInfo.LockInProbability;
        var isBossRoom = roomInfo.isBossRoom;
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
                    AddDoor(direction, oppositeRoomDoor, isLockedIn);
                } 
                else
                {
                    AddDoorwayWall(direction, roomInfo.WallType);
                }
            } else if (doorSpawnProbabilityOverride == null ? Random.value < doorSpawnProbablity : Random.value < doorSpawnProbabilityOverride)
            {
                AddDoor(direction, null, isLockedIn);
            } else
            {
                AddDoorwayWall(direction, roomInfo.WallType);
            }
        }
        if (Random.value < roomInfo.LockInProbability) { doorsClosed = true; }
    }

    #nullable enable
    public void AddDoor(Vector2 direction, GameObject? oppositeRoomDoor, bool lockedInDoor)
    {
        GameObject newDoor = doorPositions[direction].GetComponent<DoorwayGenerator>().AddDoor(gameObject, oppositeRoomDoor, lockedInDoor);
        if (doors.ContainsKey(direction))
        {
            Destroy(doors[direction]);
            doors.Remove(direction);
        }
        doors.Add(direction, newDoor);
    }

    public void AddBossDoor(Vector2 direction, GameObject? oppositeRoomDoor)
    {
        GameObject newDoor = doorPositions[direction].GetComponent<DoorwayGenerator>().AddBossDoor(gameObject, oppositeRoomDoor);
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

    public void SetBossRoomNeighbors(Dictionary<Vector2, GameObject> dungeon, Vector2 position)
    {
        foreach (Vector2 direction in Constants.directions.Values)
        {
            Vector2 oppositeDirection = direction * -1;
            Vector2 neighboorPosition = position + direction;
            if (dungeon.ContainsKey(neighboorPosition) && doors.ContainsKey(direction) && doors[direction].GetComponent<DoorController>().state == DoorState.Open)
            {
                GameObject neighbooringRoom = dungeon[neighboorPosition];
                GameObject oppositeRoomDoor = neighbooringRoom.GetComponent<DoorManager>().doors[oppositeDirection];

                DoorManager OppositeDoorManager = neighbooringRoom.GetComponent<DoorManager>();
                GameObject oppositeRoomDoorPosition = OppositeDoorManager.doorPositions[oppositeDirection];
                DoorwayGenerator oppositeRoomGenerator = oppositeRoomDoorPosition.GetComponent<DoorwayGenerator>();
                GameObject newOppositeBossDoor = oppositeRoomGenerator.AddBossDoor(neighbooringRoom, doors[direction]);

                // remove the old door from the opposite room
                if (OppositeDoorManager.doors.ContainsKey(oppositeDirection))
                {
                    Destroy(OppositeDoorManager.doors[oppositeDirection]);
                    OppositeDoorManager.doors.Remove(oppositeDirection);
                }
                OppositeDoorManager.doors.Add(oppositeDirection, newOppositeBossDoor);
            }
        }
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

    public void RoomEntered()
    {
        if(doorsClosed)
        {
            foreach (GameObject door in doors.Values)
            {
                door.GetComponent<DoorController>().RoomEntered();
            }
        }
    }
}
