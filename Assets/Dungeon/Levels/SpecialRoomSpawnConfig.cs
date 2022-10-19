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

    public class OneWellSpecialSpawn : SpecialRoomSpawnConfig {

        public static OneWellSpecialSpawn Instance = new OneWellSpecialSpawn();
        virtual public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-well"));
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

    public class OneKeySpecialSpawn : SpecialRoomSpawnConfig
    {

        public static OneKeySpecialSpawn Instance = new OneKeySpecialSpawn();
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

    public class OneLadderSpecialSpawn : SpecialRoomSpawnConfig
    {
        public static OneLadderSpecialSpawn Instance = new OneLadderSpecialSpawn();
        virtual public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-ladder"));
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

    public class OneStartSpecialSpawn : SpecialRoomSpawnConfig
    {
        public static OneStartSpecialSpawn Instance = new OneStartSpecialSpawn();
        virtual public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("1-start"));
        public int minRooms { get; set; } = 1;
        public int maxRooms { get; set; } = 1;
        public List<GameObject> RoomsThatSatisfyCondition(Dictionary<Vector2, GameObject> dungeon)
        {
            return new List<GameObject> { dungeon[new Vector2(0, 0)] };
        }
    }

    public class TwoLadderSpecialSpawn : OneLadderSpecialSpawn
    {
        new public static TwoLadderSpecialSpawn Instance = new TwoLadderSpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-ladder"));
    }

    public class TwoKeySpecialSpawn : OneKeySpecialSpawn
    {
        new public static TwoKeySpecialSpawn Instance = new TwoKeySpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-key"));
    }

    public class TwoWellSpecialSpawn : OneWellSpecialSpawn
    {
        new public static TwoWellSpecialSpawn Instance = new TwoWellSpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-well"));
    }

    public class TwoStartSpecialSpawn : OneStartSpecialSpawn
    {
        new public static TwoStartSpecialSpawn Instance = new TwoStartSpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("2-start"));
    }

     public class ThreeLadderSpecialSpawn : OneLadderSpecialSpawn
    {
        new public static ThreeLadderSpecialSpawn Instance = new ThreeLadderSpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-ladder"));
    }

    public class ThreeKeySpecialSpawn : OneKeySpecialSpawn
    {
        new public static ThreeKeySpecialSpawn Instance = new ThreeKeySpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-key"));
    }

    public class ThreeWellSpecialSpawn : OneWellSpecialSpawn
    {
        new public static ThreeWellSpecialSpawn Instance = new ThreeWellSpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-well"));
    }

    public class ThreeStartSpecialSpawn : OneStartSpecialSpawn
    {
        new public static ThreeStartSpecialSpawn Instance = new ThreeStartSpecialSpawn();
        override public Room room { get; set; } = new PreDefRoom(RoomLoader.LoadSaveRoomData("3-start"));
    }

}
