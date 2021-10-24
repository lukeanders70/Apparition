using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public static class SpawnConfigIndex
    {
        public static Dictionary<string, SpawnConfig> spawnConfigs = new Dictionary<string, SpawnConfig>()
        {
            { "empty", new EmptySpawnConfig() },
            { "easy-donut", new EasyDonutSpawnConfig() },
            { "medium-donut", new  MediumDonutSpawnConfig() },
            { "easy-inverse-donut", new EasyInverseDonutSpawnConfig() },
            { "easy-quadratic-spawn-config", new EasyQuadrantSpawnConfig() },
            { "ladder", new LadderConfig() },
            { "lava", new LavaSpawnConfig() }
        };
    }

    static class AreaRanges
    {
        public static AreaRange[] centerArea = { new AreaRange((6, 3), (18, 8)) };
        public static AreaRange[] midLeftmidRightStrips = {
            new AreaRange((4, 3), (5, 8)),
            new AreaRange((19, 3), (21, 8))
        };
        public static AreaRange[] largeOffsetTiles =
        {
            new AreaRange((2, 1), (5, 4)),
            new AreaRange((2, 6), (6, 9)),
            new AreaRange((2, 11), (5, 12)),
            new AreaRange((8, 2), (12, 5)),
            new AreaRange((8, 8), (12, 10)),

            new AreaRange((15, 1), (18, 4)),
            new AreaRange((15, 6), (19, 9)),
            new AreaRange((15, 11), (18, 12)),
            new AreaRange((21, 2), (24, 5)),
            new AreaRange((21, 8), (24, 10)),
        };
        public static AreaRange[] largeOffsetTilesCenter = { new AreaRange((8, 6), (12, 7)) };
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

    public class EmptySpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "empty-config";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = { };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = { };
    }

    public class LavaSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "lava-config";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 48,
                maxObjects = 48,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/hole", probability = 1.0f }
                },
                areaRanges = AreaRanges.largeOffsetTiles
            }
        };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 5,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/bat", probability = 1.0f },
                },
                areaRanges = AreaRanges.largeOffsetTilesCenter
            }
        };
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
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.3f }
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
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.3f }
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
                    new ObjectProbability<string> { obj = "Obstacles/stoneObstacle", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.3f }
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
                    new ObjectProbability<string> { obj = "Enemies/spinnerFast", probability = 0.0f },
                    new ObjectProbability<string> { obj = "Enemies/rusher", probability = 0.0f },
                    new ObjectProbability<string> { obj = "Enemies/blob-large", probability = 1.0f },

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
