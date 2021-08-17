using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : GridObjectManager
{
    [SerializeField]
    private int maxEnemies;
    [SerializeField]
    private int minEnemies;

    public void SetEnemies(Vector2 roomIndexPosition, StaticDungeonInfo staticDungeonInfo, RoomGrid roomGrid, RoomType roomType)
    {
        AreaRange[] enemySpawnRanges = { new AreaRange((6, 3), (18, 8)) };

        SetObjects(
            roomGrid,
            getEnemyFrequencies(roomType),
            enemySpawnRanges,
            minEnemies,
            maxEnemies
            );
    }

    private ObjectFrequency[] getEnemyFrequencies(RoomType roomType)
    {
        ObjectFrequency[] freqs = new ObjectFrequency[roomType.enemyProbabilities.Length];
        for(int i = 0; i < roomType.enemyProbabilities.Length; i++)
        {
            freqs[i] = new ObjectFrequency(roomType.enemyProbabilities[i].probability, "Enemies/" + roomType.enemyProbabilities[i].name);
        }
        return freqs;
    }
}

class EnemyInfo
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    public EnemyInfo(GameObject pref, Vector3 sPosition)
    {
        prefab = pref;
        spawnPosition = sPosition;
    }
}
