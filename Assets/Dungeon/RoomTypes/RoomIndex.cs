using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public interface Room
    {
        string WallType { get; set; }
        string Name { get; set; }
        ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; }
        float LockInProbability { get; set; }
        bool isBossRoom { get; set; }

    }

    public class PreDefRoom : Room
    {
        virtual public string Name { get; set; } = "PreDefRoom unset";

        virtual public string WallType { get; set; } = "PreDefRoom unset";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } = { };

        virtual public float LockInProbability { get; set; } = 0.0f;

        virtual public bool isBossRoom { get; set; } = false;

        public PreDefRoom(SaveRoom saveRoomData)
        {
            WallType = saveRoomData.wallType;
            LockInProbability = saveRoomData.lockIn && !saveRoomData.bossRoom ? 1.0f : 0.0f;
            SpawnConfigProbs = new ObjectProbability<SpawnConfig>[]{
                new ObjectProbability<SpawnConfig> { obj = new PreDefSpawnConfig(saveRoomData.Cells), probability = 1.0f },
            };
            isBossRoom = saveRoomData.bossRoom;
        }
    }
}
