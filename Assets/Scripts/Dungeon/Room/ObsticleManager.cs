using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleManager : GridObjectManager
{
    public string obsticlePrefabPath;
    public int minObsticles;
    public int maxObsticles;

    // Start is called before the first frame update
    public void SetObsticles(Vector2 roomIndexPosition, StaticDungeonInfo staticDungeonInfo, RoomGrid roomGrid, RoomType roomType)
    {
        AreaRange[] enemySpawnRanges = {
            new AreaRange((0,0),(11, 3)),
            new AreaRange((0,8),(11, 13)),
            new AreaRange((15,0),(25, 3)),
            new AreaRange((15,8),(25, 13))
        };
        SetObjects(
            roomGrid,
            new ObjectFrequency[] { new ObjectFrequency(1.0f, obsticlePrefabPath) },
            enemySpawnRanges,
            roomIndexPosition == Vector2.zero ? 0 : minObsticles,
            roomIndexPosition == Vector2.zero ? 0 : maxObsticles
        );
    }
}
