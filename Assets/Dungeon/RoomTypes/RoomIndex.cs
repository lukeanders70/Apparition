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
            { "basic-harder", new BasicHarderRoom() },
            { "ladder", new LadderRoom() },
            { "lava", new LavaRoom() }
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
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-donut"], probability = 0.3f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-inverse-donut"], probability = 0.3f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-quadratic-spawn-config"], probability = 0.4f }
        };
    }

    public class BasicHarderRoom : Room
    {
        public string Name { get; set; } = "basic-harder";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["medium-donut"], probability = 1.0f },
        };
    }

    public class LavaRoom : Room
    {
        public string Name { get; set; } = "lava";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["lava"], probability = 1.0f },
        };
    }

    public class LadderRoom : Room
    {
        public string Name { get; set; } = "ladder";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["ladder"], probability = 1.0f },
        };
    }
}
