using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public static class SpawnConfigIndex
    {
        public static Dictionary<string, SpawnConfig> spawnConfigs = new Dictionary<string, SpawnConfig>()
        {
            { "easy-donut", new EasyDonutSpawnConfig() },
            { "medium-donut", new  MediumDonutSpawnConfig() }
        };
    }

    public class ObjectRanges
    {
        public int maxObjects;
        public int minObjects;
        public ObjectProbability<string>[] prefabPathProbs;
        public AreaRange[] areaRanges;
    }

    public interface SpawnConfig
    {
        public string Name { get; set; }
        public ObjectRanges[] ObsticleRanges { get; set; }
        public ObjectRanges[] EnemyRanges { get; set; }
    }

    public class EasyDonutSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-donut";
        private static ObjectProbability<string>[] preabProbs = { new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 1.0f } };
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 5,
                prefabPathProbs = new ObjectProbability<string>[] { 
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 1.0f }
                },
                areaRanges = new AreaRange[]
                {
                    new AreaRange((0, 0), (11, 3)),
                    new AreaRange((0, 0), (11, 13)),
                    new AreaRange((15, 0), (25, 3)),
                    new AreaRange((15, 8), (25, 13))
                }
            }
        };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 5,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/spinner", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Enemies/spinnerFast", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Enemies/rusher", probability = 0.2f }
                },
                areaRanges = new AreaRange[]
                {
                    new AreaRange((6, 3), (18, 8))
                }
            }
        };
    }

    public class MediumDonutSpawnConfig : EasyDonutSpawnConfig
    {
        override public string Name { get; set; } = "medium-donut";

        override public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 3,
                maxObjects = 5,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/spinnerFast", probability = 0.4f },
                    new ObjectProbability<string> { obj = "Enemies/rusher", probability = 0.6f }
                },
                areaRanges = new AreaRange[]
                {
                    new AreaRange((6, 3), (18, 8))
                }
            }
        };
    }
}
