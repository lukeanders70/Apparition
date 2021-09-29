using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : GridObjectManager
{
    [SerializeField]
    private int maxEnemies;
    [SerializeField]
    private int minEnemies;

    public void SetEnemies(RoomGrid roomGrid, StaticDungeon.SpawnConfig spawnConfigInfo)
    {

        foreach (StaticDungeon.ObjectRanges oRange in spawnConfigInfo.EnemyRanges)
        {
            if (oRange.areaRanges != null)
            {
                SetObjects(
                    roomGrid,
                    oRange.prefabPathProbs,
                    oRange.areaRanges,
                    oRange.minObjects,
                    oRange.maxObjects,
                    oRange.symmetry
                );
            }
            else
            {
                SetObjects(
                    roomGrid,
                    oRange.prefabPathProbs,
                    oRange.absoluteLocations,
                    oRange.minObjects,
                    oRange.maxObjects,
                    oRange.symmetry
                );
            }
        }
    }

    public delegate void EnemyKilledEventHandler(object source, System.EventArgs args);

    public event EnemyKilledEventHandler EnemyKilled;

    public virtual void OnEnemyKilled()
    {
        Debug.Log("In Enemy Manager Killed");
        if (EnemyKilled != null)
        {
            Debug.Log("Not Null");
            EnemyKilled(this, System.EventArgs.Empty);
        }
    }
}
