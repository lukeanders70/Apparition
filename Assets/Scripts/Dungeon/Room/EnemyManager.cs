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
                    GridCell.CellObjectType.enemy,
                    oRange.prefabPathProbs,
                    oRange.areaRanges,
                    oRange.minObjects,
                    oRange.maxObjects,
                    oRange.symmetry
                );
            }
            else if (oRange.absoluteLocations != null)
            {
                SetObjects(
                    roomGrid,
                    GridCell.CellObjectType.enemy,
                    oRange.prefabPathProbs,
                    oRange.absoluteLocations,
                    oRange.minObjects,
                    oRange.maxObjects,
                    oRange.symmetry
                );
            }
            else if (oRange.preDefObjects != null)
            {
                SetObjects(
                    roomGrid,
                    GridCell.CellObjectType.enemy,
                    oRange.preDefObjects
                );
            }
            else
            {
                Debug.LogError("enemy range has no spawn data");
            }
        }
    }

    public delegate void EnemyKilledEventHandler(object source, System.EventArgs args);

    public event EnemyKilledEventHandler EnemyKilled;

    public virtual void OnEnemyKilled()
    {
        if (EnemyKilled != null)
        {
            EnemyKilled(this, System.EventArgs.Empty);
        }
    }
}
