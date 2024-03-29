using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectManager : MonoBehaviour
{

    List<ObjectInfo> objectPrefabs = new List<ObjectInfo>();
    public List<GameObject> objects = new List<GameObject>();
    public GridCell.CellObjectType objectType;

    public void SetObjects(
        RoomGrid roomGrid,
        GridCell.CellObjectType oType,
        StaticDungeon.ObjectProbability<string>[] prefabFreqs,
        AreaRange[] spawnRanges,
        int minObjects = 0,
        int maxObjects = 0,
        StaticDungeon.Symmetry symmetry = StaticDungeon.Symmetry.None
    )
    {
        objectType = oType;
        int numObjects = Random.Range(minObjects, maxObjects);
        int totalSpawnTiles = AreaRange.GetNumTilesInRanges(spawnRanges, symmetry, RoomGrid.center);
        int count = 0;

        for (int rangeIndex = 0; rangeIndex < spawnRanges.Length; rangeIndex++)
        {
            AreaRange range = spawnRanges[rangeIndex];
            int xMax = symmetry == StaticDungeon.Symmetry.LeftRight || symmetry == StaticDungeon.Symmetry.Quadrant ? Mathf.Min(range.endX, RoomGrid.center.Item1 - 1) : range.endX;
            int yMax = symmetry == StaticDungeon.Symmetry.TopBottom || symmetry == StaticDungeon.Symmetry.Quadrant ? Mathf.Min(range.endY, RoomGrid.center.Item2 - 1) : range.endY;
            for (int i = range.startX; i < xMax; i++)
            {
                for (int j = range.startY; j < yMax; j++)
                {
                    int numObjectsLeftToSpawn = numObjects - objectPrefabs.Count;
                    int numTilesLeft = totalSpawnTiles - count;
                    float spawnProbability = numTilesLeft != 0 ? (float)numObjectsLeftToSpawn / (float)numTilesLeft : 0;
                    if (spawnProbability > Random.Range(0f, 1.0f))
                    {
                        string path = StaticDungeon.Utils.ChooseFromObjectProbability(prefabFreqs);
                        AddObjectWithSymmetrty(i, j, loadPrefabFromPath(path), roomGrid, symmetry);
                    }
                    count += 1;
                }
            }
        }
    }

        public void SetObjects(
        RoomGrid roomGrid,
        GridCell.CellObjectType oType,
        StaticDungeon.ObjectProbability<string>[] prefabFreqs,
        (int, int)[] spawnLocations,
        int minObjects = 0,
        int maxObject = 0,
        StaticDungeon.Symmetry symmetry = StaticDungeon.Symmetry.None
    )
    {
        objectType = oType;
        foreach ((int, int) spawnLocation in spawnLocations)
        {
            string path = StaticDungeon.Utils.ChooseFromObjectProbability(prefabFreqs);
            Debug.Log(path);
            AddObjectWithSymmetrty(
                spawnLocation.Item1,
                spawnLocation.Item2,
                loadPrefabFromPath(path),
                roomGrid,
                symmetry
            );
        }
    }

    public void SetObjects(
        RoomGrid roomGrid,
        GridCell.CellObjectType oType,
        Dictionary<(int, int), string> spawnLocationsMap
    )
        {
            objectType = oType;
            foreach (KeyValuePair<(int, int), string> spawnLocation in spawnLocationsMap)
            {
                AddObject(
                    spawnLocation.Key.Item1,
                    spawnLocation.Key.Item2,
                    loadPrefabFromPath(spawnLocation.Value),
                    roomGrid
                );
            }
    }

    public void ClearObjects()
    {
        objectPrefabs = new List<ObjectInfo>();
        objects = new List<GameObject>();
    }

    public void AddObjectWithSymmetrty(int x, int y, GameObject prefab, RoomGrid roomGrid, StaticDungeon.Symmetry symmetry)
    {
        switch (symmetry)
        {
            case StaticDungeon.Symmetry.None:
                AddObject(x, y, prefab, roomGrid);
                break;
            case StaticDungeon.Symmetry.LeftRight:
                AddObject(x, y, prefab, roomGrid);
                AddObject((RoomGrid.center.Item1 - 1) + ((RoomGrid.center.Item1 - 1) - x), y, prefab, roomGrid);
                break;
            case StaticDungeon.Symmetry.TopBottom:
                AddObject(x, y, prefab, roomGrid);
                AddObject(x, (RoomGrid.center.Item2 - 1) + ((RoomGrid.center.Item2) - y), prefab, roomGrid);
                break;
            case StaticDungeon.Symmetry.Quadrant:
                AddObject(x, y, prefab, roomGrid);
                AddObject((RoomGrid.center.Item1 - 1) + ((RoomGrid.center.Item1 - 1) - x), y, prefab, roomGrid);
                AddObject(x, (RoomGrid.center.Item2) + ((RoomGrid.center.Item2) - y), prefab, roomGrid);
                AddObject((RoomGrid.center.Item1 - 1) + ((RoomGrid.center.Item1 - 1) - x), (RoomGrid.center.Item2) + ((RoomGrid.center.Item2) - y), prefab, roomGrid);
                break;
        }
    }

    public void AddObject(int x, int y, GameObject prefab, RoomGrid roomGrid)
    {
        Vector2? location = roomGrid.addObject(prefab, objectType, x, y);
        if (location != null && prefab != null)
        {
            objectPrefabs.Add(new ObjectInfo(
                prefab,
                (Vector3)location,
                (x, y)
            ));
        }
    }
    private GameObject loadPrefabFromPath(string path)
    {
        if(path != "") {
            return Resources.Load<GameObject>("prefabs/" + path);
        }
        return null;
    }

    public void SpawnObjects(RoomGrid roomGrid)
    {
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            GameObject newObject = Instantiate(
                objectPrefabs[i].prefab,
                gameObject.transform,
                false
            );
            newObject.transform.localPosition = objectPrefabs[i].spawnPosition;
            if(newObject.GetComponent<DynamicSpriteSetter>() != null)
            {
                newObject.GetComponent<DynamicSpriteSetter>().SpawnCallback(roomGrid, objectPrefabs[i].roomGridLocation);
            }
            objects.Add(newObject);
        }
    }

    public void ExitRoom()
    {
        if (objects.Count == objectPrefabs.Count)
        {
            List<ObjectInfo> newObjectPrefabs = new List<ObjectInfo>();
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    newObjectPrefabs.Add(objectPrefabs[i]);
                    Destroy(objects[i]);
                }
            }
            objectPrefabs = newObjectPrefabs;
            objects = new List<GameObject>();
        }
        else
        {
            Debug.LogError($"object count {objects.Count} did not equal prefab count {objectPrefabs.Count} in room");
        }
    }

    public int ObjectCount()
    {
        int c = 0;
        foreach (GameObject o in objects)
            c += ObjectDestroyed(o) ? 0 : 1;
        return c;
    }

    private bool ObjectDestroyed(GameObject o)
    {
        return o == null || o.tag == "Destroyed";
    }
}

public class ObjectFrequency
{
    public float freq;
    public string prefabPath;
    public ObjectFrequency(float f, string p)
    {
        freq = f;
        prefabPath = p;
    }
}

class ObjectInfo
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    public (int, int) roomGridLocation;
    public ObjectInfo(GameObject pref, Vector3 sPosition, (int, int) index)
    {
        prefab = pref;
        spawnPosition = sPosition;
        roomGridLocation = index;
    }
}
