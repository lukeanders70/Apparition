using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public interface SpecialRoomSpawnConfig
    {
        public Room room { get; set; }
        public int minRooms { get; set; }
        public int maxRooms { get; set; }
        public List<GameObject> RoomsThatSatisfyCondition(Dictionary<Vector2, GameObject> dungeon);
    }

    public class StoneWellSpecialSpawn : SpecialRoomSpawnConfig {

        public static StoneWellSpecialSpawn Instance = new StoneWellSpecialSpawn();
        public Room room { get; set; } = WellStone.Instance;
        public int minRooms { get; set; } = 1;
        public int maxRooms { get; set; } = 1;
        public List<GameObject> RoomsThatSatisfyCondition(Dictionary<Vector2, GameObject> dungeon)
        {
            var possible = new List<GameObject>();
            foreach (Vector2 key in dungeon.Keys)
            {
                float manDistance = Mathf.Abs(key.x) + Mathf.Abs(key.y);
                if(manDistance > 2)
                {
                    possible.Add(dungeon[key]);
                }
            }
            return possible;
        }
    }

    public class StoneKeySpecialSpawn : SpecialRoomSpawnConfig
    {

        public static StoneKeySpecialSpawn Instance = new StoneKeySpecialSpawn();
        virtual public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-key"));
        public int minRooms { get; set; } = 1;
        public int maxRooms { get; set; } = 1;
        public List<GameObject> RoomsThatSatisfyCondition(Dictionary<Vector2, GameObject> dungeon)
        {
            var possible = new List<GameObject>();
            foreach (Vector2 key in dungeon.Keys)
            {
                float manDistance = Mathf.Abs(key.x) + Mathf.Abs(key.y);
                
                if (manDistance > 1 && dungeon[key].GetComponent<DoorManager>().getAccessibleDirections().Count > 1)
                {
                    possible.Add(dungeon[key]);
                }
            }
            return possible;
        }
    }

    public class StoneExitSpecialSpawn : SpecialRoomSpawnConfig
    {
        public static StoneExitSpecialSpawn Instance = new StoneExitSpecialSpawn();
        virtual public Room room { get; set; } = Exit1Room.Instance;
        public int minRooms { get; set; } = 1;
        public int maxRooms { get; set; } = 1;
        public List<GameObject> RoomsThatSatisfyCondition(Dictionary<Vector2, GameObject> dungeon)
        {
            float maxDistance = 0;
            Vector2 maxKey = Vector2.zero;
            float maxSingleAxisDistance = 0; // tie breaker

            foreach (Vector2 key in dungeon.Keys)
            {
                float manDistance = Mathf.Abs(key.x) + Mathf.Abs(key.y);
                float singleAxisDistance = Mathf.Max(Mathf.Abs(key.x), Mathf.Abs(key.y));
                if ((manDistance > maxDistance) || (manDistance == maxDistance && singleAxisDistance > maxSingleAxisDistance))
                {
                    maxDistance = manDistance;
                    maxKey = key;
                    maxSingleAxisDistance = singleAxisDistance;
                }
            }
            return new List<GameObject> { dungeon[maxKey] };
        }
    }

    public class SandstoneExitSpecialSpawn : StoneExitSpecialSpawn
    {
        new public static SandstoneExitSpecialSpawn Instance = new SandstoneExitSpecialSpawn();
        override public Room room { get; set; } = Exit2Room.Instance;
    }

    public class SandstoneKeySpecialSpawn : StoneKeySpecialSpawn
    {
        new public static SandstoneKeySpecialSpawn Instance = new SandstoneKeySpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-key"));
    }

    public class RockExitSpecialSpawn : StoneExitSpecialSpawn
    {
        new public static RockExitSpecialSpawn Instance = new RockExitSpecialSpawn();
        override public Room room { get; set; } = Exit3Room.Instance;
    }

    public class RockKeySpecialSpawn : StoneKeySpecialSpawn
    {
        new public static RockKeySpecialSpawn Instance = new RockKeySpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-key"));
    }

}
