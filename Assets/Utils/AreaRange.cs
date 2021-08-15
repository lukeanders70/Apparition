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

    public static int GetNumTilesInRanges(AreaRange[] ranges)
    {
        int totalSpawnTiles = 0;
        for (int rangeIndex = 0; rangeIndex < ranges.Length; rangeIndex++)
        {
            totalSpawnTiles += ranges[rangeIndex].numTilesInRange;
        }
        return totalSpawnTiles;
    }
}
