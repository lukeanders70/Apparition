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
                    oRange.prefabPathProbs,
                    oRange.areaRanges,
                    oRange.minObjects,
                    oRange.maxObjects,
                    oRange.symmetry
                );
            } else
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
}
