using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public static class SpawnConfigIndex
    {
        public static Dictionary<string, SpawnConfig> spawnConfigs = new Dictionary<string, SpawnConfig>()
        {
            // Shared
            { "empty", new EmptySpawnConfig() },
            { "ladder", new LadderConfig() },
            { "well", new WellConfig() },
            // Level 1
            { "easy-donut", new EasyDonutSpawnConfig() },
            { "wolf-den", new  WolfDen() },
            { "blob-den", new  BlobDen() },
            { "easy-inverse-donut", new EasyInverseDonutSpawnConfig() },
            { "easy-quadratic-spawn-config", new EasyQuadrantSpawnConfig() },
            { "easy-walled", new EasyWalledRoomConfig() },
            { "easy-walled-alt", new AltWalledRoomConfig() },
            { "easy-walled-strips", new WalledRoomStrips() },
            { "short-wall-maze", new ShortWallMazeConfig() },
            // Level 2
            { "easy-donut-shrine", new EasyDonutSandShrine() },
            { "medium-maze-sand-walls", new MediumMazeSandWalls() },
            // Level 3
            { "lava-maze", new LavaSpawnConfig() },
            { "easy-scattered-rocks", new EasyScatteredRocksConfig() },
        };
    }

    static class AreaRanges
    {
        public static AreaRange[] largerCenter = { new AreaRange((3, 2), (21, 9)) };
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
        public static AreaRange[] Circuit =
        {
            // top
            new AreaRange((3, 2), (11, 3)),
            new AreaRange((15, 2), (23, 3)),

            // left
            new AreaRange((3, 3), (4, 5)),
            new AreaRange((3, 8), (4, 10)),

            //right
            new AreaRange((22, 3), (23, 5)),
            new AreaRange((22, 8), (23, 10)),

            // bottom
            new AreaRange((3, 10), (11, 11)),
            new AreaRange((15, 10), (23, 11)),
        };

        public static AreaRange[] TopLeftMiniCircuit =
        {
            // top
            new AreaRange((3, 2), (11, 3)),
            // left
            new AreaRange((3, 3), (4, 4)),
            // right
            new AreaRange((10, 2), (11, 4)),
            // bottom
            new AreaRange((3, 4), (11, 5)),
        };

        public static AreaRange[] QuadrentStrips =
{
            // top left
            new AreaRange((3, 3), (11, 4)),

            // top right
            new AreaRange((15, 3), (23, 4)),

            // bottom left
            new AreaRange((3, 9), (11, 10)),

            // bottom right
            new AreaRange((15, 9), (23, 10)),
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
        public Dictionary<(int, int), string> preDefObjects;
    }

    public interface SpawnConfig
    {
        public string Name { get; set; }
        public ObjectRanges[] ObsticleRanges { get; set; }
        public ObjectRanges[] EnemyRanges { get; set; }
        public ObjectRanges[] MiscRanges { get; set; }
    }

    public class PreDefSpawnConfig : SpawnConfig
    {

        virtual public string Name { get; set; } = "PreDefSpawnConfig Unset";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = { };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = { };
        virtual public ObjectRanges[] MiscRanges { get; set; } = { };

        public PreDefSpawnConfig(List<GridCell> Cells)
        {
            var objPrefabDict = new Dictionary<(int, int), string>();
            var enemyPrefabDict = new Dictionary<(int, int), string>();
            var miscPrefabDict = new Dictionary<(int, int), string>();
            foreach (GridCell cell in Cells)
            {
                if(cell.objectType == GridCell.CellObjectType.obstacle)
                {
                    objPrefabDict.Add((cell.primaryIndex.x, cell.primaryIndex.y), "Obstacles/" + cell.objectName);
                } else if(cell.objectType == GridCell.CellObjectType.misc)
                {
                    miscPrefabDict.Add((cell.primaryIndex.x, cell.primaryIndex.y), "Misc/" + cell.objectName);
                }
                else {
                    enemyPrefabDict.Add((cell.primaryIndex.x, cell.primaryIndex.y), "Enemies/" + cell.objectName);
                }
            }
            ObsticleRanges = new ObjectRanges[] {
                new ObjectRanges {
                    preDefObjects = objPrefabDict
                }
            };
            EnemyRanges = new ObjectRanges[] {
                new ObjectRanges {
                    preDefObjects = enemyPrefabDict
                }
            };
            MiscRanges = new ObjectRanges[]
            {
                new ObjectRanges
                {
                    preDefObjects = miscPrefabDict
                }
            };
        }
    }

    // Shared //
    public class EmptySpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "empty-config";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = { };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = { };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
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

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class WellConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "well";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 1,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/heartWell", probability = 1.0f }
                },
                absoluteLocations = new (int, int)[] { (12, 5) }
            }
        };

        virtual public ObjectRanges[] EnemyRanges { get; set; } = { };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    // Level 1 //

    public class ShortWallMazeConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "short-wall-maze-config";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 30,
                maxObjects = 30,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 1.0f }
                },
                areaRanges = AreaRanges.largeOffsetTiles
            }
        };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 2,
                maxObjects = 4,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/bat", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Enemies/rusher", probability = 0.3f },
                },
                areaRanges = AreaRanges.centerArea
            }
        };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class EasyWalledRoomConfig : EasyDonutSpawnConfig
    {
        override public string Name { get; set; } = "walled-room-config";
        override public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 38,
                maxObjects = 44,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stone-wall", probability = 1.0f }
                },
                areaRanges = AreaRanges.Circuit
            },
            new ObjectRanges {
                minObjects = 0,
                maxObjects = 4,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 1.0f }
                },
                areaRanges = AreaRanges.centerArea
            }
        };
    }

    public class AltWalledRoomConfig : EasyDonutSpawnConfig
    {
        override public string Name { get; set; } = "alt-walled-room-config";
        override public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 60,
                maxObjects = 72,
                symmetry = Symmetry.Quadrant,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stone-wall", probability = 1.0f }
                },
                areaRanges = AreaRanges.TopLeftMiniCircuit
            }
        };
    }

    public class WalledRoomStrips : EasyDonutSpawnConfig
    {
        override public string Name { get; set; } = "walled-room-strips-config";
        override public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 28,
                maxObjects = 32,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/stone-wall", probability = 1.0f }
                },
                areaRanges = AreaRanges.QuadrentStrips
            },
            new ObjectRanges {
                minObjects = 0,
                maxObjects = 4,
                symmetry = Symmetry.Quadrant,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.3f }
                },
                areaRanges = AreaRanges.centerArea
            }
        };
    }

    public class EasyQuadrantSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-quadrant";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 12,
                maxObjects = 28,
                symmetry = Symmetry.Quadrant,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/TallStone", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.2f }
                },
                areaRanges = AreaRanges.largerCenter
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

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class EasyDonutSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-donut";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 8,
                maxObjects = 20,
                symmetry = Symmetry.LeftRight,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/TallStone", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.2f }
                },
                areaRanges = AreaRanges.largerCenter
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

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class EasyInverseDonutSpawnConfig : SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-inverse-donut";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 8,
                maxObjects = 20,
                symmetry = Symmetry.TopBottom,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/TallStone", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.2f }
                },
                areaRanges = AreaRanges.largerCenter
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

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class BlobDen : SpawnConfig
    {
        public string Name { get; set; } = "blob-den";

        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 8,
                maxObjects = 20,
                symmetry = Symmetry.LeftRight,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/TallStone", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.2f }
                },
                areaRanges = AreaRanges.largerCenter
            },
            new ObjectRanges {
                minObjects = 0,
                maxObjects = 1,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/treasureChest", probability = 1.0f }
                },
                absoluteLocations = new (int, int)[] { (12, 5) }
            },
        };

        public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 2,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/blob-large", probability = 1.0f },
                },
                areaRanges = AreaRanges.midLeftmidRightStrips
            },
            new ObjectRanges {
                minObjects = 3,
                maxObjects = 6,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/spinnerFast", probability = 3.0f },
                    new ObjectProbability<string> { obj = "Enemies/spinner", probability = 7.0f },
                },
                areaRanges = AreaRanges.midLeftmidRightStrips
            }
        };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class WolfDen : SpawnConfig
    {
        public string Name { get; set; } = "wolf-den";

        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 8,
                maxObjects = 20,
                symmetry = Symmetry.LeftRight,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/ShortStone", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/TallStone", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Obstacles/torch", probability = 0.2f }
                },
                areaRanges = AreaRanges.largerCenter
            },
            new ObjectRanges {
                minObjects = 0,
                maxObjects = 1,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/treasureChest", probability = 1.0f }
                },
                absoluteLocations = new (int, int)[] { (12, 5) }
            },
        };

        public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 1,
                maxObjects = 1,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/big-wolf", probability = 1.0f },
                },
                areaRanges = AreaRanges.midLeftmidRightStrips
            },
            new ObjectRanges {
                minObjects = 3,
                maxObjects = 5,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/spinnerFast", probability = 0.1f },
                    new ObjectProbability<string> { obj = "Enemies/spinner", probability = 0.4f },
                    new ObjectProbability<string> { obj = "Enemies/rusher", probability = 0.5f },
                },
                areaRanges = AreaRanges.midLeftmidRightStrips
            }
        };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    // Level 2 //

    public class EasyDonutSandShrine : SpawnConfig
    {
        virtual public string Name { get; set; } = "level-2-donut";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 8,
                maxObjects = 20,
                symmetry = Symmetry.Quadrant,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/sandstoneShort", probability = 0.8f },
                    new ObjectProbability<string> { obj = "Obstacles/sandstonePeg", probability = 0.2f },
                },
                areaRanges = AreaRanges.centerArea
            }
        };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 3,
                maxObjects = 8,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/spinner", probability = 0.5f },
                    new ObjectProbability<string> { obj = "Enemies/pyramid-eye", probability = 0.5f }
                },
                areaRanges = AreaRanges.largerCenter
            }
        };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class MediumMazeSandWalls : SpawnConfig
    {
        virtual public string Name { get; set; } = "level-2-donut";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 40,
                maxObjects = 40,
                symmetry = Symmetry.Quadrant,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/sandstoneWall", probability = 1.0f },
                },
                areaRanges = AreaRanges.largeOffsetTiles
            }
        };
        virtual public ObjectRanges[] EnemyRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 2,
                maxObjects = 4,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Enemies/pyramid-eye", probability = 1.0f }
                },
                areaRanges = AreaRanges.largerCenter
            }
        };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    // Level 3 // 

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
                areaRanges = AreaRanges.centerArea
            }
        };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }

    public class EasyScatteredRocksConfig: SpawnConfig
    {
        virtual public string Name { get; set; } = "easy-scattered-rocks";
        virtual public ObjectRanges[] ObsticleRanges { get; set; } = {
            new ObjectRanges {
                minObjects = 10,
                maxObjects = 15,
                symmetry = Symmetry.None,
                prefabPathProbs = new ObjectProbability<string>[] {
                    new ObjectProbability<string> { obj = "Obstacles/rock", probability = 0.7f },
                    new ObjectProbability<string> { obj = "Obstacles/stalagmite", probability = 0.3f }
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
                    new ObjectProbability<string> { obj = "Enemies/mole", probability = 0.5f },
                    new ObjectProbability<string> { obj = "Enemies/bat", probability = 0.5f },
                },
                areaRanges = AreaRanges.centerArea
            }
        };

        virtual public ObjectRanges[] MiscRanges { get; set; } = { };
    }
}
