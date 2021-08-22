using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : GridObjectManager
{
    [SerializeField]
    private int maxEnemies;
    [SerializeField]
    private int minEnemies;

    public void SetEnemies(Vector2 roomIndexPosition, RoomGrid roomGrid, StaticDungeon.SpawnConfig spawnConfigInfo)
    {

        foreach (StaticDungeon.ObjectRanges oRange in spawnConfigInfo.EnemyRanges)
        {
            SetObjects(
                roomGrid,
                oRange.prefabPathProbs,
                oRange.areaRanges,
                oRange.minObjects,
                oRange.maxObjects
            );
        }
    }
}
