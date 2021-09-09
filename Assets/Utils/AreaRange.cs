using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaRange
{
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public int numTilesInRange = 0;

    public AreaRange((int, int) topLeftPoint, (int, int) bottomRightPoint)
    {
        startX = topLeftPoint.Item1;
        startY = topLeftPoint.Item2;
        endX = bottomRightPoint.Item1;
        endY = bottomRightPoint.Item2;

        numTilesInRange = System.Math.Max((endX - startX) * (endY - startY), 0);
    }

    public static int GetNumTilesInRanges(AreaRange[] ranges, StaticDungeon.Symmetry symmetry, (int, int) center)
    {
        int totalSpawnTiles = 0;
        for (int rangeIndex = 0; rangeIndex < ranges.Length; rangeIndex++)
        {
            totalSpawnTiles += ranges[rangeIndex].getNumberOfTilesInRangeWithSymmetry(symmetry, center);
        }
        return totalSpawnTiles;
    }

    public (int, int) getMaxValuesWithSymmetry(StaticDungeon.Symmetry symmetry, (int, int) center)
    {
        int xMax = symmetry == StaticDungeon.Symmetry.LeftRight || symmetry == StaticDungeon.Symmetry.Quadrant ? Mathf.Min(this.endX, center.Item1 - 1) : this.endX;
        int yMax = symmetry == StaticDungeon.Symmetry.TopBottom || symmetry == StaticDungeon.Symmetry.Quadrant ? Mathf.Min(this.endY, center.Item2 - 1) : this.endY;
        return (xMax, yMax);
    }

    public int getNumberOfTilesInRangeWithSymmetry(StaticDungeon.Symmetry symmetry, (int, int) center)
    {
        (int, int) newMaxes = this.getMaxValuesWithSymmetry(symmetry, center);
        return System.Math.Max((newMaxes.Item1 - startX) * (newMaxes.Item2 - startY), 0);
    }
}
