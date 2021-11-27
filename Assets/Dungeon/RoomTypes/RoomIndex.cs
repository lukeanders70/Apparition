using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class RoomIndex
    {
        public static Dictionary<string, Room> rooms = new Dictionary<string, Room>()
        {
            { "entry-room", new EntryRoom() },
            { "basic", new BasicRoom() },
            { "basic-moss", new BasicMossRoom() },
            { "basic-harder", new BasicHarderRoom() },
            { "ladder", new LadderRoom() },
            { "lava", new LavaRoom() },
            { "stone-maze", new StoneMaze() }
        };
    }

    public interface Room
    {
        string WallType { get; set; }
        string Name { get; set; }
        ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; }
        float LockInProbability { get; set; }

    }

    public class EntryRoom : Room
    {
        public string Name { get; set; } = "entry-room";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } = {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["short-wall-maze"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class BasicRoom : Room
    {
        public string Name { get; set; } = "basic-room";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-donut"], probability = 0.2f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-inverse-donut"], probability = 0.1f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-quadratic-spawn-config"], probability = 0.3f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-walled"], probability = 0.2f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-walled-alt"], probability = 0.1f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-walled-strips"], probability = 0.1f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
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
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["blob-den"], probability = 0.5f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["wolf-den"], probability = 0.5f },
        };

        virtual public float LockInProbability { get; set; } = 1.0f;
    }

    public class LavaRoom : Room
    {
        public string Name { get; set; } = "lava";

        public string WallType { get; set; } = "cave";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["lava"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class StoneMaze : Room
    {
        public string Name { get; set; } = "stone-maze";

        public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["short-wall-maze"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class LadderRoom : Room
    {
        public string Name { get; set; } = "ladder";

        public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["ladder"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }
}
