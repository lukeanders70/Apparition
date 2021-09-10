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
            { "easy-inverse-donut", new EasyInverseDonutSpawnConfig() },
            { "easy-quadratic-spawn-config", new EasyQuadrantSpawnConfig() },
            { "ladder", new LadderConfig() }
        };
    }

    static class AreaRanges
    {
        public static AreaRange[] centerArea = { new AreaRange((6, 3), (18, 8)) };
        public static AreaRange[] midLeftmidRightStrips = {
            new AreaRange((4, 3), (5, 8)),
            new AreaRange((19, 3), (21, 8))
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
        public (int, int)[] absoluteLocations;
    }

    public interface SpawnConfig
    {
        public string Name { get; set; }
        public ObjectRanges[] ObsticleRanges { get; set; }
        public ObjectRanges[] EnemyRanges { get; set; }
    }

    public class EasyQuadrantSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-quadrant";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 2,
                maxObjects = 12,
                symmetry = Symmetry.Quadrant,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 1.0f }
                },
                areaRanges = AreaRanges.midLeftmidRightStrips
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
                areaRanges = AreaRanges.centerArea
            }
        };
    }

    public class EasyDonutSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-donut";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 5,
                symmetry = Symmetry.LeftRight,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 1.0f }
                },
                areaRanges = AreaRanges.midLeftmidRightStrips
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
                areaRanges = AreaRanges.centerArea
            }
        };
    }

    public class EasyInverseDonutSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-inverse-donut";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 5,
                symmetry = Symmetry.TopBottom,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 1.0f }
                },
                areaRanges = AreaRanges.centerArea
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
                areaRanges = AreaRanges.midLeftmidRightStrips
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
                    new ObjectProbability<string> { obj = "Enemies/spinnerFast", probability = 0.3f },
                    new ObjectProbability<string> { obj = "Enemies/rusher", probability = 0.6f },
                    new ObjectProbability<string> { obj = "Enemies/blob-large", probability = 0.1f },

                },
                areaRanges = AreaRanges.midLeftmidRightStrips
            }
        };
    }

    public class LadderConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "ladder";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 1,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ladder", probability = 1.0f }
                },
                absoluteLocations = new (int, int)[] { (11, 4) }
            }
        };

        virtual public ObjectRanges[] EnemyRanges { get; set; } = { };
    }
}
