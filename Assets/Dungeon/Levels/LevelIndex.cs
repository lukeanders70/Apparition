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
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("test")), probability = 1.0f },
/*            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-stone-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-stone-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-stone-3")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-stone-4")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-moss-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-moss-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-moss-3")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-easy-moss-4")), probability = 1.0f }*/
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-stone-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-stone-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-broken-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-broken-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-ledge-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-ledge-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-ledge-3")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-ledge-lock-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-moss-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-medium-moss-2")), probability = 1.0f },
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-cobbled-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-cobbled-3")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-cobbled-lock-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-fancy-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-fancy-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-fancy-3")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-fancy-lock-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-fancy-lock-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-moss-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-moss-lock-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-stone-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-stone-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-hard-stone-3")), probability = 1.0f },

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
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-easy-pyramid-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-easy-pyramid-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-easy-sandstone-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-easy-sandstone-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-easy-sandstone-3")), probability = 1.0f },
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-medium-pyramid-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-medium-pyramid-lock-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-medium-sandstone-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-medium-sandstone-2")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-medium-sandstone-lock-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-medium-sandstone-lock-2")), probability = 1.0f },
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-hard-pyramid-1")), probability = 1.0f },
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
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-easy-cave-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-easy-cave-2")), probability = 1.0f },
        };
        public ObjectProbability<Room>[] MediumRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-medium-cave-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-medium-cave-1")), probability = 1.0f },
        };
        public ObjectProbability<Room>[] FarRooms { get; set; } =
        {
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-hard-cave-1")), probability = 1.0f },
            new ObjectProbability<Room> { obj = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-hard-cave-lock-1")), probability = 1.0f },
        };

        public SpecialRoomSpawnConfig[] SpecialRooms { get; set; } = {
            StoneWellSpecialSpawn.Instance,
            RockExitSpecialSpawn.Instance
        };
    }
}
