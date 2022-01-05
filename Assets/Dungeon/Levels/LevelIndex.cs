using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class LevelIndex
    {
        public static Level[] levels = {
            new Level1(),
            new Level2(),
            new Level3(),
        };
    }

    public interface Level
    {
        public string Name { get; set; }
        public Room EntryRoom { get; set; }
        public ObjectProbability<Room>[] NearRooms { get; set; }
        public ObjectProbability<Room>[] MediumRooms { get; set; }
        public ObjectProbability<Room>[] FarRooms { get; set; }
        public Room ExitRoom { get; set; }
    }

    public class Level1 : Level
    {
        public string Name { get; set; } = "level1";
        public Room EntryRoom { get; set; } = RoomIndex.rooms["entry1"];
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["stoneEasy"], probability = 0.0f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["mossyStoneEasy"], probability = 0.0f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["brokenStoneEasy"], probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["tapestryStoneMiniBoss"], probability = 0.1f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["mossyStoneEasy"], probability = 0.2f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["stoneMaze"], probability = 0.2f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["cobbledStoneEasy"], probability = 0.2f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["stoneLedgeEasy"], probability = 0.3f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["tapestryStoneMiniBoss"], probability = 1.0f },
        };

        public Room ExitRoom { get; set; } = RoomIndex.rooms["exit1"];
    }

    public class Level2 : Level
    {
        public string Name { get; set; } = "level2";
        public Room EntryRoom { get; set; } = RoomIndex.rooms["entry2"];
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["sandstoneSlopeEasy"], probability = 0.5f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["sandstoneEasy"], probability = 0.5f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["sandstoneSlopeEasy"], probability = 0.5f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["sandstoneEasy"], probability = 0.5f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["sandstoneSlopeEasy"], probability = 0.5f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["sandstoneEasy"], probability = 0.5f }
        };

        public Room ExitRoom { get; set; } = RoomIndex.rooms["exit2"];
    }

    public class Level3 : Level
    {
        public string Name { get; set; } = "level3";
        public Room EntryRoom { get; set; } = RoomIndex.rooms["entry3"];
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["rockLavaMaze"], probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["rockLavaMaze"], probability = 1.0f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["rockLavaMaze"], probability = 1.0f }
        };

        public Room ExitRoom { get; set; } = RoomIndex.rooms["exit3"];
    }
}
