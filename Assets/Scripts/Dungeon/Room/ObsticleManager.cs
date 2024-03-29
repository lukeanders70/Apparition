using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleManager : GridObjectManager
{
    public string obsticlePrefabPath;
    public int minObsticles;
    public int maxObsticles;

    // Start is called before the first frame update
    public void SetObsticles(RoomGrid roomGrid, StaticDungeon.SpawnConfig spawnConfigInfo)
    {
        foreach (StaticDungeon.ObjectRanges oRange in spawnConfigInfo.ObsticleRanges)
        {
            if (oRange.areaRanges != null)
            {
                SetObjects(
                    roomGrid,
                    GridCell.CellObjectType.obstacle,
                    oRange.prefabPathProbs,
                    oRange.areaRanges,
                    oRange.minObjects,
                    oRange.maxObjects,
                    oRange.symmetry
                );
            } else if (oRange.absoluteLocations != null)
            {
                SetObjects(
                    roomGrid,
                    GridCell.CellObjectType.obstacle,
                    oRange.prefabPathProbs,
                    oRange.absoluteLocations,
                    oRange.minObjects,
                    oRange.maxObjects,
                    oRange.symmetry
                );
            } else if (oRange.preDefObjects != null)
            {
                SetObjects(
                    roomGrid,
                    GridCell.CellObjectType.obstacle,
                    oRange.preDefObjects
                );
            } else
            {
                Debug.LogError("obsticle range has no spawn data");
            }

        }

        foreach (StaticDungeon.ObjectRanges oRange in spawnConfigInfo.MiscRanges)
        {
            if (oRange.preDefObjects != null)
            {
                SetObjects(
                    roomGrid,
                    GridCell.CellObjectType.misc,
                    oRange.preDefObjects
                );
            }
        }

    }
}
