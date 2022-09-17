using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject RoomPrefab;
    [SerializeField]
    private float roomHeight;
    [SerializeField]
    private float roomWidth;
    [SerializeField]
    private float numRooms;
    [SerializeField]
    private Animator levelTransition;

    private GameObject level;

    private void Awake()
    {
        GenerateDungeon(StaticDungeon.LevelIndex.levels[DungeonStateInfo.levelIndex]);
    }

    public bool MoveDown()
    {
        if (StaticDungeon.LevelIndex.levels.Length > DungeonStateInfo.levelIndex + 1)
        {
            DungeonStateInfo.levelIndex = DungeonStateInfo.levelIndex + 1;
            GenerateDungeon(StaticDungeon.LevelIndex.levels[DungeonStateInfo.levelIndex]);
            return true;
        }
        Debug.LogError("Failed to move down to level index: " + (DungeonStateInfo.levelIndex + 1));
        return false;
    }

    private void GenerateDungeon(StaticDungeon.Level levelInfo)
    {
        replaceLevel(levelInfo.Name);

        Dictionary<Vector2, GameObject> dungeon = new Dictionary<Vector2, GameObject>();
        List<Vector2> edge = new List<Vector2>();

        GameObject firstRoom = AddRoom(Vector2.zero, dungeon, levelInfo, 1f);
        UpdateEdge(new Vector2(0, 0), firstRoom, edge, dungeon);

        for (int i = 0; i < numRooms - 1; i++)
        {
            int index = Random.Range(0, edge.Count);
            Vector2 indexPositionToAdd = edge[index];

            GameObject newRoom;
            if (edge.Count > 1 || i == numRooms - 1)
            {
                newRoom = AddRoom(indexPositionToAdd, dungeon, levelInfo, null);
            }
            else
            {
                newRoom = AddRoom(indexPositionToAdd, dungeon, levelInfo, 1f);
            }
            UpdateEdge(indexPositionToAdd, newRoom, edge, dungeon);
            edge.Remove(indexPositionToAdd);
        }
        SetSpecialRooms(dungeon, levelInfo.SpecialRooms);
    }

    private void UpdateEdge(Vector2 roomPosition, GameObject newRoom, List<Vector2> edge, Dictionary<Vector2, GameObject> dungeon)
    {
        foreach (Vector2 neighboorDirection in newRoom.GetComponent<DoorManager>().getAccessibleDirections())
        {
            if (!edge.Contains(roomPosition + neighboorDirection) && !dungeon.ContainsKey(roomPosition + neighboorDirection))
            {
                edge.Add(roomPosition + neighboorDirection);
            }
        }
    }

    private void SetSpecialRooms(Dictionary<Vector2, GameObject> dungeon, StaticDungeon.SpecialRoomSpawnConfig[] specialRoomConfigs)
    {
        foreach(StaticDungeon.SpecialRoomSpawnConfig specialRoomConfig in specialRoomConfigs)
        {
            var possibleRoomsToOverride = specialRoomConfig.RoomsThatSatisfyCondition(dungeon);
            var numRoomsToAdd = Random.Range(specialRoomConfig.minRooms, specialRoomConfig.maxRooms);
            var roomsToOverride = AIHelpers.ChooseN(numRoomsToAdd, possibleRoomsToOverride);
            foreach(GameObject roomToOverride in roomsToOverride)
            {
                var roomController = roomToOverride.GetComponent<RoomController>();
                roomController.SetRoomInfo(dungeon, roomController.position, specialRoomConfig.room);
            }
        }
    }

    private GameObject AddRoom(
        Vector2 indexPositionToAdd,
        Dictionary<Vector2, GameObject> dungeon,
        StaticDungeon.Level levelInfo,
        float? doorProbability
        )
    {
        GameObject newRoom = Instantiate(
            RoomPrefab,
            level.transform.position + new Vector3(indexPositionToAdd.x * roomWidth, indexPositionToAdd.y * roomHeight),
            level.transform.rotation
        );
        newRoom.transform.parent = level.transform;
        newRoom.GetComponent<RoomController>().SetupRoom(dungeon, indexPositionToAdd, levelInfo, doorProbability);
        dungeon.Add(indexPositionToAdd, newRoom);

        return newRoom;
    }

    private void replaceLevel(string newLevelName)
    {
        if (level != null)
        {
            Destroy(level);
        } 
        level = new GameObject(newLevelName);
        level.transform.parent = transform;
        level.transform.position = Vector3.zero;
    }
}
