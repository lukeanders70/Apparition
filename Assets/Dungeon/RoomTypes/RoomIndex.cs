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
            { "basic-moss", new BasicMossRoom() },
            { "basic-harder", new BasicHarderRoom() },
            { "ladder", new LadderRoom() },
            { "lava", new LavaRoom() }
        };
    }

    public interface Room
    {
        string WallType { get; set; }
        string Name { get; set; }
        ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; }
        
    }

    public class BasicRoom : Room
    {
        public string Name { get; set; } = "basic-room";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-donut"], probability = 0.3f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-inverse-donut"], probability = 0.3f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-quadratic-spawn-config"], probability = 0.4f }
        };
    }

    public class BasicMossRoom : BasicRoom
    {
        override public string WallType { get; set; } = "moss";
    }

    public class BasicHarderRoom : Room
    {
        public string Name { get; set; } = "basic-harder";

        public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["medium-donut"], probability = 1.0f },
        };
    }

    public class LavaRoom : Room
    {
        public string Name { get; set; } = "lava";

        public string WallType { get; set; } = "cave";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["lava"], probability = 1.0f },
        };
    }

    public class LadderRoom : Room
    {
        public string Name { get; set; } = "ladder";

        public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["ladder"], probability = 1.0f },
        };
    }
}
