using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleManager : GridObjectManager
{
    public string obsticlePrefabPath;
    public int minObsticles;
    public int maxObsticles;

    // Start is called before the first frame update
    public void SetObsticles(Vector2 roomIndexPosition, RoomGrid roomGrid, StaticDungeon.SpawnConfig spawnConfigInfo)
    {
        foreach (StaticDungeon.ObjectRanges oRange in spawnConfigInfo.ObsticleRanges)
        {
            
            SetObjects(
                roomGrid,
                oRange.prefabPathProbs,
                oRange.areaRanges,
                roomIndexPosition == Vector2.zero ? 0 : oRange.minObjects,
                roomIndexPosition == Vector2.zero ? 0 : oRange.maxObjects
            );
        }

    }
}
