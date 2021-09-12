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
        };
    }

    public interface Level
    {
        public string Name { get; set; }
        public ObjectProbability<Room>[] NearRooms { get; set; }
        public ObjectProbability<Room>[] MediumRooms { get; set; }
        public ObjectProbability<Room>[] FarRooms { get; set; }
        public Room ExitRoom { get; set; }
    }

    public class Level1 : Level
    {
        public string Name { get; set; } = "level1";
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic"], probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic"], probability = 0.5f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic-harder"], probability = 0.5f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic-harder"], probability = 0.3f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["lava"], probability = 0.7f }
        };

        public Room ExitRoom { get; set; } = RoomIndex.rooms["ladder"];
    }

    public class Level2 : Level
    {
        public string Name { get; set; } = "level2";
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic"], probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic"], probability = 0.5f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic-harder"], probability = 0.5f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic-harder"], probability = 0.3f },
            new ObjectProbability<Room> { obj = RoomIndex.rooms["lava"], probability = 0.7f }
        };

        public Room ExitRoom { get; set; } = RoomIndex.rooms["ladder"];
    }
}
