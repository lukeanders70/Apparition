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

    private List<Vector2> directions = new List<Vector2>() {
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(0, 1)
    };

    public Dictionary<Vector2, GameObject> rooms = new Dictionary<Vector2, GameObject>();

    private void Awake()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Dictionary<Vector2, GameObject> dungeon = new Dictionary<Vector2, GameObject>();
        List<Vector2> edge = new List<Vector2>();

        GameObject firstRoom = AddRoom(transform.position, dungeon, edge, 1);
        UpdateEdge(new Vector2(0, 0), firstRoom, edge, dungeon);

        for (int i = 0; i < numRooms - 1; i++)
        {
            int index = Random.Range(0, edge.Count);
            Vector2 positionToAdd = edge[index];

            GameObject newRoom;
            if (edge.Count > 1 || i == numRooms - 1)
            {
                newRoom = AddRoom(positionToAdd, dungeon, edge, null);
            }
            else
            {
                newRoom = AddRoom(positionToAdd, dungeon, edge, 1);
            }
            UpdateEdge(positionToAdd, newRoom, edge, dungeon);
            edge.Remove(positionToAdd);

        }
    }

    private void UpdateEdge(Vector2 roomPosition, GameObject newRoom, List<Vector2> edge, Dictionary<Vector2, GameObject> dungeon)
    {
        foreach (Vector2 neighboorDirection in newRoom.GetComponent<Doors>().getAccessibleDirections())
        {
            if (!edge.Contains(roomPosition + neighboorDirection) && !dungeon.ContainsKey(roomPosition + neighboorDirection))
            {
                edge.Add(roomPosition + neighboorDirection);
            }
        }
    }

    private GameObject AddRoom(Vector2 positionToAdd, Dictionary<Vector2, GameObject> dungeon, List<Vector2> edge, float? doorProbability)
    {
        GameObject newRoom = GameObject.Instantiate(
            RoomPrefab,
            transform.position + new Vector3(positionToAdd.x * roomWidth, positionToAdd.y * roomHeight),
            transform.rotation
        );
        newRoom.transform.parent = transform;
        Doors RoomSetup = newRoom.GetComponent<Doors>();
        if (doorProbability != null) { RoomSetup.doorSpawnProbablity = (float)doorProbability; }
        RoomSetup.SetupDoors(dungeon, positionToAdd);
        dungeon[positionToAdd] = newRoom;

        return newRoom;
    }
}
