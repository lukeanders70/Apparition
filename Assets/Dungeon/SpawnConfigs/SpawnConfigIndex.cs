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
            { "medium-donut", new  MediumDonutSpawnConfig() },
            { "easy-inverse-donut", new EasyInverseDonutSpawnConfig() }
        };
    }

    public enum Symmetry
    {
        None,
        LeftRight,
        TopBottom,
        Quadrant
    }

    public class ObjectRanges
    {
        public int maxObjects;
        public int minObjects;
        public Symmetry symmetry;
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
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 5,
                maxObjects = 5,
                symmetry = Symmetry.LeftRight,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 1.0f }
                },
                areaRanges = new AreaRange[]
                {
                    new AreaRange((0, 0), (10, 3)),
                    new AreaRange((0, 8), (10, 13))
                }
            }
        };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 5,
                symmetry = Symmetry.None,
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

    public class EasyInverseDonutSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-inverse-donut";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 5,
                maxObjects = 5,
                symmetry = Symmetry.TopBottom,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 1.0f }
                },
                areaRanges = new AreaRange[]
                {
                    new AreaRange((6, 3), (18, 6))
                }
            }
        };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 5,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/spinner", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Enemies/spinnerFast", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Enemies/rusher", probability = 0.2f }
                },
                areaRanges = new AreaRange[]
                {
                    new AreaRange((4, 3), (5, 8)),
                    new AreaRange((19, 3), (21, 8))
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
                symmetry = Symmetry.None,
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
