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

    }

    public class PreDefRoom : Room
    {
        virtual public string Name { get; set; } = "PreDefRoom unset";

        virtual public string WallType { get; set; } = "PreDefRoom unset";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } = {};

        virtual public float LockInProbability { get; set; } = 0.0f;

        public PreDefRoom(SaveRoom saveRoomData)
        {
            WallType = saveRoomData.wallType;
            LockInProbability = saveRoomData.lockIn ? 1.0f : 0.0f;
            SpawnConfigProbs = new ObjectProbability<SpawnConfig>[]{
                new ObjectProbability<SpawnConfig> { obj = new PreDefSpawnConfig(saveRoomData.Cells), probability = 1.0f },
            };
        }
    }
    // Level 1
    public class Entry1Room : Room
    {
        public static Entry1Room Instance = new Entry1Room();
        virtual public string Name { get; set; } = "Entry 1";

        virtual public string WallType { get; set; } = "basic";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } = {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["empty"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class StoneEasyRoom : Room
    {
        public static StoneEasyRoom Instance = new StoneEasyRoom();
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
        new public static MossyStoneEasyRoom Instance = new MossyStoneEasyRoom();
        override public string Name { get; set; } = "Mossy Stone Easy";
        override public string WallType { get; set; } = "moss";
    }

    public class StoneLedgeEasyRoom : StoneEasyRoom
    {
        new public static StoneLedgeEasyRoom Instance = new StoneLedgeEasyRoom();
        override public string Name { get; set; } = "Stone Ledge Easy";
        override public string WallType { get; set; } = "ledge";
    }

    public class BrokenStoneEasyRoom : StoneEasyRoom
    {
        new public static BrokenStoneEasyRoom Instance = new BrokenStoneEasyRoom();
        override public string Name { get; set; } = "Broken Stone Easy";
        override public string WallType { get; set; } = "broken";
    }

    public class CobbledStoneEasyRoom : StoneEasyRoom
    {
        new public static CobbledStoneEasyRoom Instance = new CobbledStoneEasyRoom();
        override public string Name { get; set; } = "Cobbled Stone Easy";
        override public string WallType { get; set; } = "cobbled";
    }

    public class TapestryStoneMiniBossRoom : Room
    {
        public static TapestryStoneMiniBossRoom Instance = new TapestryStoneMiniBossRoom();
        public string Name { get; set; } = "Tapestry Stone Mini-Boss";

        public string WallType { get; set; } = "fancy";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["blob-den"], probability = 0.5f },
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["wolf-den"], probability = 0.5f },
        };

        virtual public float LockInProbability { get; set; } = 1.0f;
    }

    public class WellStone : Room
    {
        public static WellStone Instance = new WellStone();
        public string Name { get; set; } = "Heart Well Stone";

        public string WallType { get; set; } = "moss";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["well"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class StoneMazeRoom : Room
    {
        public static StoneMazeRoom Instance = new StoneMazeRoom();
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
        public static Exit1Room Instance = new Exit1Room();
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
        new public static Entry2Room Instance = new Entry2Room();
        override public string Name { get; set; } = "Entry 2";
        override public string WallType { get; set; } = "sandstone";
    }

    public class SandstoneEasyRoom :Room
    {
        public static SandstoneEasyRoom Instance = new SandstoneEasyRoom();
        virtual public string Name { get; set; } = "Sandstone Easy";
        virtual public string WallType { get; set; } = "sandstone";

        virtual public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
{
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-donut-shrine"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class SandstoneSlopeEasyRoom : Room
    {
        public static SandstoneSlopeEasyRoom Instance = new SandstoneSlopeEasyRoom();
        virtual public string Name { get; set; } = "Sandstone Slope Easy";
        virtual public string WallType { get; set; } = "pyramid";

        virtual public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
{
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-donut-shrine"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class SandstoneMediumMazeRoom : Room
    {
        public static SandstoneMediumMazeRoom Instance = new SandstoneMediumMazeRoom();
        virtual public string Name { get; set; } = "Sandstone Slope Maze Medium";
        virtual public string WallType { get; set; } = "pyramid";

        virtual public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
{
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["medium-maze-sand-walls"], probability = 1.0f },
        };

        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class Exit2Room : Exit1Room
    {
        new public static Exit2Room Instance = new Exit2Room();
        override public string Name { get; set; } = "Exit 2";

        override public string WallType { get; set; } = "sandstone";
    }

    // Level 3 //

    public class Entry3Room : Entry1Room
    {
        new public static Entry3Room Instance = new Entry3Room();
        override public string Name { get; set; } = "Entry 3";
        override public string WallType { get; set; } = "cave";
    }
    public class RockLavaMazeRoom : Room
    {
        public static RockLavaMazeRoom Instance = new RockLavaMazeRoom();
        public string Name { get; set; } = "Rock Lava Maze";

        public string WallType { get; set; } = "cave";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["lava-maze"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class RockEasyRoom : Room
    {
        public static RockEasyRoom Instance = new RockEasyRoom();
        public string Name { get; set; } = "Rock Easy";

        public string WallType { get; set; } = "cave";
        public ObjectProbability<SpawnConfig>[] SpawnConfigProbs { get; set; } =
        {
            new ObjectProbability<SpawnConfig> { obj = SpawnConfigIndex.spawnConfigs["easy-scattered-rocks"], probability = 1.0f },
        };
        virtual public float LockInProbability { get; set; } = 0.0f;
    }

    public class Exit3Room : Exit1Room
    {
        new public static Exit3Room Instance = new Exit3Room();
        override public string Name { get; set; } = "exit 3";

        override public string WallType { get; set; } = "cave";
    }
}
