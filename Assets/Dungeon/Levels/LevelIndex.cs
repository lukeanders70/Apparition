using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class LevelIndex
    {
        public static Level[] levels = {
            new Level1(),
        };
    }

    public interface Level
    {
        public ObjectProbability<Room>[] NearRooms { get; set; }
        public ObjectProbability<Room>[] MediumRooms { get; set; }
        public ObjectProbability<Room>[] FarRooms { get; set; }
    }

    public class Level1 : Level
    {
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
            new ObjectProbability<Room> { obj = RoomIndex.rooms["basic-harder"], probability = 1.0f }
        };
    }
}
