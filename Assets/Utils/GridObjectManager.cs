using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectManager : MonoBehaviour
{

    List<ObjectInfo> objectPrefabs = new List<ObjectInfo>();
    List<GameObject> objects = new List<GameObject>();
    // Start is called before the first frame update
    public void SetObjects(
        RoomGrid roomGrid,
        StaticDungeon.ObjectProbability<string>[] prefabFreqs,
        AreaRange[] spawnRanges,
        int minObjects = 0,
        int maxObject = 0
    )
    {

        int numObjects = Random.Range(maxObject, minObjects);
        int totalSpawnTiles = AreaRange.GetNumTilesInRanges(spawnRanges);
        int count = 0;

        for (int rangeIndex = 0; rangeIndex < spawnRanges.Length; rangeIndex++)
        {
            AreaRange range = spawnRanges[rangeIndex];
            for (int i = range.startX; i < range.endX; i++)
            {
                for (int j = range.startY; j < range.endY; j++)
                {
                    int numEnemiesLeftToSpawn = numObjects - objectPrefabs.Count;
                    int numTilesLeft = totalSpawnTiles - count;
                    float spawnProbability = numTilesLeft != 0 ? (float)numEnemiesLeftToSpawn / (float)numTilesLeft : 0;
                    if (spawnProbability > Random.Range(0f, 1.0f))
                    {
                        AddObject(i, j, loadPrefabFromPath(StaticDungeon.Utils.ChooseFromObjectProbability(prefabFreqs)), roomGrid);
                    }
                    count += 1;
                }
            }
        }
    }

    public void AddObject(int x, int y, GameObject prefab, RoomGrid roomGrid)
    {
        Vector2? location = roomGrid.addObject(prefab, x, y);
        if (location != null && prefab != null)
        {
            objectPrefabs.Add(new ObjectInfo(
                prefab,
                (Vector3)location
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

    public void SpawnObjects()
    {
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            GameObject newObject = Instantiate(
                objectPrefabs[i].prefab,
                gameObject.transform,
                false
            );
            newObject.transform.localPosition = objectPrefabs[i].spawnPosition;
            objects.Add(newObject);
        }
    }

    public void ExitRoom()
    {
        if (objects.Count == objects.Count)
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
    public ObjectInfo(GameObject pref, Vector3 sPosition)
    {
        prefab = pref;
        spawnPosition = sPosition;
    }
}
