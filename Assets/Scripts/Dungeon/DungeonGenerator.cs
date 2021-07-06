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

    public Dictionary<Vector2, GameObject> rooms = new Dictionary<Vector2, GameObject>();

    private void Awake()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Dictionary<Vector2, GameObject> dungeon = new Dictionary<Vector2, GameObject>();
        List<Vector2> edge = new List<Vector2>();

        GameObject firstRoom = AddRoom(Vector2.zero, dungeon, 1f);
        UpdateEdge(new Vector2(0, 0), firstRoom, edge, dungeon);

        for (int i = 0; i < numRooms - 1; i++)
        {
            int index = Random.Range(0, edge.Count);
            Vector2 indexPositionToAdd = edge[index];

            GameObject newRoom;
            if (edge.Count > 1 || i == numRooms - 1)
            {
                newRoom = AddRoom(indexPositionToAdd, dungeon, null);
            }
            else
            {
                newRoom = AddRoom(indexPositionToAdd, dungeon, 1f);
            }
            UpdateEdge(indexPositionToAdd, newRoom, edge, dungeon);
            edge.Remove(indexPositionToAdd);

        }
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

    private GameObject AddRoom(Vector2 indexPositionToAdd, Dictionary<Vector2, GameObject> dungeon, float? doorProbability)
    {
        GameObject newRoom = Instantiate(
            RoomPrefab,
            transform.position + new Vector3(indexPositionToAdd.x * roomWidth, indexPositionToAdd.y * roomHeight),
            transform.rotation
        );
        newRoom.transform.parent = transform;
        newRoom.GetComponent<RoomController>().SetupRoom(dungeon, indexPositionToAdd, doorProbability);
        dungeon.Add(indexPositionToAdd, newRoom);

        return newRoom;
    }
}
