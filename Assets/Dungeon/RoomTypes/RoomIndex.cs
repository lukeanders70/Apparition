using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class RoomIndex
    {
        public static Dictionary<string, Room> rooms = new Dictionary<string, Room>()
        {
            { "entry-room-level-1", new EntryRoomLevel1() },
            { "entry-room-level-2", new EntryRoomLevel2() },
            { "basic", new BasicRoom() },
            { "cobbled", new BasicCobbledRoom() },
            { "ledge", new BasicLedgeRoom() },
            { "pyramid", new PyramidRoom() },
            { "sandstone", new BasicSandstoneRoom() },
            { "basic-moss", new BasicMossRoom() },
            { "broken", new BasicBrokeneRoom() },
            { "mini-boss", new MiniBossRoom() },
            { "exit-room-level-1", new ExitRoomLevel1() },
            { "exit-room-level-2", new ExitRoomLevel2() },
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

    public class EntryRoomLevel1 : Room
    {
        virtual public string Name { get; set; } = "entry-room-level-1";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } = {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["empty"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class EntryRoomLevel2 : EntryRoomLevel1
    {
        override public string Name { get; set; } = "entry-room-level-2";
        override public string WallType { get; set; } = "sandstone";
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

    public class BasicLedgeRoom : BasicRoom
    {
        override public string WallType { get; set; } = "ledge";
    }

    public class BasicBrokeneRoom : BasicRoom
    {
        override public string WallType { get; set; } = "broken";
    }

    public class BasicCobbledRoom : BasicRoom
    {
        override public string WallType { get; set; } = "cobbled";
    }

    public class PyramidRoom : BasicRoom
    {
        override public string WallType { get; set; } = "pyramid";
    }

    public class BasicSandstoneRoom : BasicRoom
    {
        override public string WallType { get; set; } = "sandstone";
    }


    public class MiniBossRoom : Room
    {
        public string Name { get; set; } = "mini-boss";

        public string WallType { get; set; } = "fancy";
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

    public class ExitRoomLevel1 : Room
    {
        virtual public string Name { get; set; } = "exit-room-level-1";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["ladder"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class ExitRoomLevel2 : ExitRoomLevel1
    {
        override public string Name { get; set; } = "exit-room-level-2";

        override public string WallType { get; set; } = "sandstone";
    }
}
