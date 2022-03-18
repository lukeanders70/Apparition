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

        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; }
    }

    public class Level1 : Level
    {
        public string Name { get; set; } = "Crumbling Catacombs 1";
        public Room EntryRoom { get; set; } = Entry1Room.Instance;
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = StoneEasyRoom.Instance, probability = 0.0f },
            new ObjectProbability<Room> { obj = MossyStoneEasyRoom.Instance, probability = 0.0f },
            new ObjectProbability<Room> { obj = BrokenStoneEasyRoom.Instance, probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = TapestryStoneMiniBossRoom.Instance, probability = 0.1f },
            new ObjectProbability<Room> { obj = MossyStoneEasyRoom.Instance, probability = 0.2f },
            new ObjectProbability<Room> { obj = StoneMazeRoom.Instance, probability = 0.2f },
            new ObjectProbability<Room> { obj = CobbledStoneEasyRoom.Instance, probability = 0.2f },
            new ObjectProbability<Room> { obj = StoneLedgeEasyRoom.Instance, probability = 0.3f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = TapestryStoneMiniBossRoom.Instance, probability = 1.0f },
        };

        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; } =
        {
            StoneWellSpecialSpawn.Instance,
            StoneExitSpecialSpawn.Instance
        };
    }

    public class Level1point5 : Level
    {
        public string Name { get; set; } = "Crumbling Catacombs 2";
        public Room EntryRoom { get; set; } = Entry1Room.Instance;
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = StoneEasyRoom.Instance, probability = 0.0f },
            new ObjectProbability<Room> { obj = MossyStoneEasyRoom.Instance, probability = 0.0f },
            new ObjectProbability<Room> { obj = BrokenStoneEasyRoom.Instance, probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = TapestryStoneMiniBossRoom.Instance, probability = 0.1f },
            new ObjectProbability<Room> { obj = MossyStoneEasyRoom.Instance, probability = 0.2f },
            new ObjectProbability<Room> { obj = StoneMazeRoom.Instance, probability = 0.2f },
            new ObjectProbability<Room> { obj = CobbledStoneEasyRoom.Instance, probability = 0.2f },
            new ObjectProbability<Room> { obj = StoneLedgeEasyRoom.Instance, probability = 0.3f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = TapestryStoneMiniBossRoom.Instance, probability = 1.0f },
        };

        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; } =
        {
            StoneWellSpecialSpawn.Instance,
            StoneExitSpecialSpawn.Instance
        };
    }

    public class Level2 : Level
    {
        public string Name { get; set; } = "Sand Sea 1";
        public Room EntryRoom { get; set; } = Entry2Room.Instance;
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = SandstoneSlopeEasyRoom.Instance, probability = 0.5f },
            new ObjectProbability<Room> { obj = SandstoneEasyRoom.Instance, probability = 0.5f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = SandstoneMediumMazeRoom.Instance, probability = 0.7f },
            new ObjectProbability<Room> { obj = SandstoneSlopeEasyRoom.Instance, probability = 0.3f },
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = SandstoneMediumMazeRoom.Instance, probability = 1.0f },
        };
        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; } =
{           StoneWellSpecialSpawn.Instance,
            SandstoneExitSpecialSpawn.Instance
        };
    }

    public class Level2point5 : Level
    {
        public string Name { get; set; } = "Sand Sea 2";
        public Room EntryRoom { get; set; } = Entry2Room.Instance;
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = SandstoneSlopeEasyRoom.Instance, probability = 0.5f },
            new ObjectProbability<Room> { obj = SandstoneEasyRoom.Instance, probability = 0.5f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = SandstoneMediumMazeRoom.Instance, probability = 0.7f },
            new ObjectProbability<Room> { obj = SandstoneSlopeEasyRoom.Instance, probability = 0.3f },
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = SandstoneMediumMazeRoom.Instance, probability = 1.0f },
        };
        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; } =
{           StoneWellSpecialSpawn.Instance,
            SandstoneExitSpecialSpawn.Instance
        };
    }

    public class Level3 : Level
    {
        public string Name { get; set; } = "Boiling Burrow 1";
        public Room EntryRoom { get; set; } = Entry3Room.Instance;
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RockEasyRoom.Instance, probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RockEasyRoom.Instance, probability = 1.0f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RockLavaMazeRoom.Instance, probability = 1.0f }
        };

        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; } = {
            StoneWellSpecialSpawn.Instance,
            RockExitSpecialSpawn.Instance
        };
    }

    public class Level3point5 : Level
    {
        public string Name { get; set; } = "Boiling Burrow 2";
        public Room EntryRoom { get; set; } = Entry3Room.Instance;
        public ObjectProbability<Room>[] NearRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RockEasyRoom.Instance, probability = 1.0f }
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RockEasyRoom.Instance, probability = 1.0f }
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = RockLavaMazeRoom.Instance, probability = 1.0f }
        };

        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; } = {
            StoneWellSpecialSpawn.Instance,
            RockExitSpecialSpawn.Instance
        };
    }
}
