using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class RoomIndex
    {
        public static Dictionary<string, Room> rooms = new Dictionary<string, Room>()
        {
            { "basic", new BasicRoom() },
        };
    }

    public interface Room
    {
        string Name { get; set; }
        ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; }
    }

    public class BasicRoom : Room
    {
        public string Name { get; set; } = "basic-room";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-donut"], probability = 0.5f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["medium-donut"], probability = 0.5f }
        };
    }
}
