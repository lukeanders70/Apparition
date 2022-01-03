using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class RoomIndex
    {
        public static Dictionary<string, Room> rooms = new Dictionary<string, Room>()
        {
            // Shared

            // Level 1
            { "entry1", new Entry1Room() },
            { "stoneEasy", new StoneEasyRoom() },
            { "cobbledStoneEasy", new CobbledStoneEasyRoom() },
            { "stoneLedgeEasy", new StoneLedgeEasyRoom() },
            { "mossyStoneEasy", new MossyStoneEasyRoom() },
            { "brokenStoneEasy", new BrokenStoneEasyRoom() },
            { "tapestryStoneMiniBoss", new TapestryStoneMiniBossRoom() },
            { "exit1", new Exit1Room() },
            { "stoneMaze", new StoneMazeRoom() },

            // Level 2
            { "entry2", new Entry2Room() },
            { "sandstoneEasy", new SandstoneEasyRoom() },
            { "sandstoneSlopeEasy", new SandstoneSlopeEasyRoom() },
            { "exit2", new Exit2Room() },

            // Level 2
            { "entry3", new Entry3Room() },
            { "rockLavaMaze", new RockLavaMazeRoom() },
            { "exit3", new Exit3Room() },
        };
    }

    public interface Room
    {
        string WallType { get; set; }
        string Name { get; set; }
        ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; }
        float LockInProbability { get; set; }

    }
    // Level 1
    public class Entry1Room : Room
    {
        virtual public string Name { get; set; } = "Entry 1";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } = {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["empty"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class StoneEasyRoom : Room
    {
        virtual public string Name { get; set; } = "Stone Easy";

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

    public class MossyStoneEasyRoom : StoneEasyRoom
    {
        override public string Name { get; set; } = "Mossy Stone Easy";
        override public string WallType { get; set; } = "moss";
    }

    public class StoneLedgeEasyRoom : StoneEasyRoom
    {
        override public string Name { get; set; } = "Stone Ledge Easy";
        override public string WallType { get; set; } = "ledge";
    }

    public class BrokenStoneEasyRoom : StoneEasyRoom
    {
        override public string Name { get; set; } = "Broken Stone Easy";
        override public string WallType { get; set; } = "broken";
    }

    public class CobbledStoneEasyRoom : StoneEasyRoom
    {
        override public string Name { get; set; } = "Cobbled Stone Easy";
        override public string WallType { get; set; } = "cobbled";
    }

    public class TapestryStoneMiniBossRoom : Room
    {
        public string Name { get; set; } = "Tapestry Stone Mini-Boss";

        public string WallType { get; set; } = "fancy";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["blob-den"], probability = 0.5f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["wolf-den"], probability = 0.5f },
        };

        virtual public float LockInProbability { get; set; } = 1.0f;
    }

    public class StoneMazeRoom : Room
    {
        public string Name { get; set; } = "Stone Maze";

        public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["short-wall-maze"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class Exit1Room : Room
    {
        virtual public string Name { get; set; } = "Exit 1";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["ladder"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    // Level 2 //

    public class Entry2Room : Entry1Room
    {
        override public string Name { get; set; } = "Entry 2";
        override public string WallType { get; set; } = "sandstone";
    }

    public class SandstoneEasyRoom : StoneEasyRoom
    {
        override public string Name { get; set; } = "Sandstone Easy";
        override public string WallType { get; set; } = "sandstone";
    }

    public class SandstoneSlopeEasyRoom : StoneEasyRoom
    {
        override public string Name { get; set; } = "Sandstone Slope Easy";
        override public string WallType { get; set; } = "pyramid";
    }

    public class Exit2Room : Exit1Room
    {
        override public string Name { get; set; } = "Exit 2";

        override public string WallType { get; set; } = "sandstone";
    }

    // Level 3 //

    public class Entry3Room : Entry1Room
    {
        override public string Name { get; set; } = "Entry 3";
        override public string WallType { get; set; } = "cave";
    }
    public class RockLavaMazeRoom : Room
    {
        public string Name { get; set; } = "Rock Lava Maze";

        public string WallType { get; set; } = "cave";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["lava"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class Exit3Room : Exit1Room
    {
        override public string Name { get; set; } = "exit 3";

        override public string WallType { get; set; } = "cave";
    }
}
