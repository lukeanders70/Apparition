using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class SpecialRoomSpawnConfigIndex
    {
        public static Dictionary<string, SpecialRoomSpawnConfig> specialRoomSpawnConfigs = new Dictionary<string, SpecialRoomSpawnConfig>()
        {
            // Shared
            { "stoneWell", new StoneWellSpecialSpawn() },
            { "stoneExit", new StoneExitSpecialSpawn() },
            { "sandstoneExit", new SandstoneExitSpecialSpawn() },
            { "rockExit", new RockExitSpecialSpawn() },
        };
    }
    public interface SpecialRoomSpawnConfig
    {
        public Room room { get; set; }
        public int minRooms { get; set; }
        public int maxRooms { get; set; }
        public List<GameObject> RoomsThatSatisfyCondition(Dictionary<Vector2, GameObject> dungeon);
    }

    public class StoneWellSpecialSpawn : SpecialRoomSpawnConfig {
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

    public class StoneExitSpecialSpawn : SpecialRoomSpawnConfig
    {
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
        override public Room room { get; set; } = Exit2Room.Instance;
    }

    public class RockExitSpecialSpawn : StoneExitSpecialSpawn
    {
        override public Room room { get; set; } = Exit3Room.Instance;
    }

}
