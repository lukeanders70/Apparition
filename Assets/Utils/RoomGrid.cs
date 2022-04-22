using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid
{
#nullable enable
    public static int cellSize = 8;
    public static (int, int) center = (13, 6);
    public static IntVector2 dimensions = new IntVector2(26, 13);
    public GridCell?[,] objectLocations = new GridCell[26, 13];

    public bool IndexInBounds((int, int) index)
    {
        return index.Item1 >= 0 && index.Item1 < objectLocations.GetLength(0) && index.Item2 >= 0 && index.Item2 < objectLocations.GetLength(1);
    }

    public Vector2 GetLocationCell(int xIndex, int yIndex, GameObject o)
    {
        IntVector2 size = GetSize(o);
        Vector2 offset = GetOffset(o);
        float xVal = ((xIndex - center.Item1) + (size.x / 2.0f)) - offset.x;
        float yVal = -(((yIndex - center.Item2) + (size.y / 2.0f)) + offset.y);
        return new Vector2(xVal, yVal);
    }

    public IntVector2 GetCellFromLocation(Vector2 location)
    {
        Vector2 distanceFromTopLeft = new Vector2(location.x + (center.Item1), (center.Item2) - location.y);
        return GetCellFromTopLeftOffset(distanceFromTopLeft);
    }

    public IntVector2 GetCellFromTopLeftOffset(Vector2 topLeftOffset)
    {
        return new IntVector2(
            Mathf.FloorToInt(topLeftOffset.x),
            Mathf.FloorToInt(topLeftOffset.y)
        );
    }

    public static Vector2 GetLocationFromCell(IntVector2 cellIndex)
    {
        return new Vector2(
            cellIndex.x - (center.Item1) + 0.5f,
            -(cellIndex.y - (center.Item2) + 0.5f)
        );
    }

    public static Vector2 GetLocationFromPoint(Vector2 position)
    {
        return new Vector2(
            position.x - (center.Item1),
            -(position.y - (center.Item2))
        );
    }

    public void print()
    {
        var s = "";
        for (int i = 0; i < objectLocations.GetLength(1); i++)
        {
            for (int j = 0; j < objectLocations.GetLength(0); j++)
            {
                var obj = objectLocations[j, i];
                if (obj != null)
                {
                    if (obj.primaryIndex != null)
                    {
                        s += " " + obj.primaryIndex;
                    }
                    else
                    {
                        s += " (p, p)";
                    }
                }
                else
                {
                    s += "  (x, x)";
                }
            }
            s += "\n";
        }
        Debug.Log(s);
    }
    //
    public Vector2? addObject(GameObject o, GridCell.CellObjectType oType, int xIndex, int yIndex)
    {
        IntVector2 size = GetSize(o);
        if (isEmpty(new IntVector2(xIndex, yIndex), size, false))
        {
            objectLocations[xIndex, yIndex] = new GridCell(o, GridCell.CellType.primary, oType, size, new IntVector2(xIndex, yIndex));
            for (int i = xIndex; i < xIndex + size.x; i++)
            {
                for (int j = yIndex; j < yIndex + size.y; j++)
                {
                    if (!(i == xIndex && j == yIndex))
                    {
                        objectLocations[i, j] = new GridCell(o, GridCell.CellType.overflow, oType, size, new IntVector2(xIndex, yIndex));
                    }
                }
            }
            return GetLocationCell(xIndex, yIndex, o);
        }
        return null;
    }

    private IntVector2? findPrimaryIndex(int xIndex, int yIndex)
    {
        if (objectLocations[xIndex, yIndex] == null)
        {
            return null;
        } else if (objectLocations[xIndex, yIndex]?.type == GridCell.CellType.primary)
        {
            return new IntVector2(xIndex, yIndex);
        } else if (objectLocations[xIndex, yIndex]?.primaryIndex != null)
        {
            return objectLocations[xIndex, yIndex]?.primaryIndex;
        } else
        {
            return null;
        }
    }

    public GameObject? getObject(int xIndex, int yIndex)
    {
        GridCell? cell = objectLocations[xIndex, yIndex];
        if (cell == null)
        {
            return null;
        }
        else if (cell.type == GridCell.CellType.primary)
        {
            return cell.prefab;
        }
        else if (cell.primaryIndex != null)
        {
            var primaryIndex = cell.primaryIndex;
            return objectLocations[primaryIndex.x, primaryIndex.y]?.prefab;
        }
        else
        {
            return null;
        }
    }

    public GameObject? removeObject(int xIndex, int yIndex)
    {
        IntVector2? primaryIndex = findPrimaryIndex(xIndex, yIndex);
        if (primaryIndex == null)
        {
            return null;
        }
        GridCell? primary = objectLocations[primaryIndex.x, primaryIndex.y];
        GameObject? o = primary?.prefab;
        for (int i = primaryIndex.x; i < primaryIndex.x + primary?.size.x; i++)
        {
            for (int j = primaryIndex.y; j < primaryIndex.y + primary?.size.y; j++)
            {
                objectLocations[i, j] = null;
            }
        }
        return o;
    }

    public void clear()
    {
        for (int i = 0; i < objectLocations.GetLength(0); i++)
        {
            for (int j = 0; j < objectLocations.GetLength(1); j++)
            {
                removeObject(i, j);
            }
        }
        print();
    }
    public bool isEmpty(IntVector2 index, IntVector2? size, bool obstacleOnly)
    {
        if (size == null)
        {
            size = new IntVector2(1, 1);
        }
        if (!IsInRange(index.x, index.y, size))
        {
            return false;
        }
        for (int i = index.x; i < index.x + size.x; i++)
        {
            for (int j = index.y; j < index.y + size.y; j++)
            {
                if (objectLocations[i, j] != null && (!obstacleOnly || objectLocations[i, j]!.objectType == GridCell.CellObjectType.obstacle))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool isEmptyCenter(IntVector2 index, Vector2 size, bool obstacleOnly)
    {
        var xDirSize = Mathf.CeilToInt((size.x / 2.0f) - 0.5f);
        var yDirSize = Mathf.CeilToInt((size.y / 2.0f) - 0.5f);
        var xMin = index.x - xDirSize;
        var xMax = index.x + xDirSize;
        var yMin = index.y - yDirSize;
        var yMax = index.y + yDirSize;
        if (xMin < 0 || xMax >= dimensions.x || yMin < 0 || yMax >= dimensions.y) {
            return false;
        }
        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                if (objectLocations[x, y] != null && (!obstacleOnly || objectLocations[x, y]!.objectType == GridCell.CellObjectType.obstacle))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool isPointEmptyCenter(Vector2 point, Vector2 size, bool obstacleOnly) {
        var xDirSize = size.x / 2.0f;
        var yDirSize = size.y / 2.0f;

        var topLeft = GetCellFromTopLeftOffset(new Vector2(point.x - xDirSize, point.y - yDirSize));
        var bottomRight = GetCellFromTopLeftOffset(new Vector2(point.x + xDirSize, point.y + yDirSize));

        if (topLeft.x < 0 || bottomRight.x >= dimensions.x || topLeft.y < 0 || topLeft.y >= dimensions.y)
        {
            return false;
        }
        for (int x = topLeft.x; x <= bottomRight.x; x++)
        {
            for (int y = topLeft.y; y <= bottomRight.y; y++)
            {
                if (IndexInBounds((x, y)) && objectLocations[x, y] != null && (!obstacleOnly || objectLocations[x, y]!.objectType == GridCell.CellObjectType.obstacle))
                {
                    return false;
                }
            }
        }
        return true;
    }


    public bool isEmpty(IntVector2 index, GameObject o, bool obstacleOnly = false)
    {
        var collider = o.GetComponent<BoxCollider2D>();
        var topLeftPosition = GetLocationFromCell(index) + ((collider.offset - (0.5f * collider.size)) * new Vector2(1, -1));
        var topLeftIndex = GetCellFromLocation(topLeftPosition);
        return isEmpty(
            topLeftIndex,
            new IntVector2((int)System.Math.Ceiling(collider.size.x + collider.offset.x), (int)System.Math.Ceiling(collider.size.y - collider.offset.y)),
            obstacleOnly
        );
    }

    private bool IsInRange(int xIndex, int yIndex, IntVector2 size)
    {
        return !(
            xIndex < 0 ||
            yIndex < 0 ||
            xIndex + size.x > objectLocations.GetLength(0) || 
            yIndex + size.y > objectLocations.GetLength(1)
            );
    }

    private IntVector2 GetSize(GameObject o)
    {
        BoxCollider2D oCollider;
        bool hasCollider = o.TryGetComponent(out oCollider);
        if (hasCollider)
        {
            return new IntVector2((int) System.Math.Ceiling(oCollider.size.x), (int) System.Math.Ceiling(oCollider.size.y));
        } else
        {
            return new IntVector2(1, 1);
        }
    }
    private Vector2 GetOffset(GameObject o)
    {
        BoxCollider2D oCollider;
        bool hasCollider = o.TryGetComponent(out oCollider);
        if (hasCollider)
        {
            return oCollider.offset;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
[System.Serializable]
public class GridCell
{
    public enum CellType
    {
        primary, // top left cell containing object
        overflow, // cell an object overflows into because of its size
    }

    public enum CellObjectType { 
        enemy,
        obstacle
    }

    [System.NonSerialized]
    public GameObject prefab;
    public string objectName;
    public CellType type;
    public CellObjectType objectType;
    public IntVector2 size;
    public IntVector2? primaryIndex = null;
    public GridCell(GameObject o, CellType cellType, CellObjectType cellObjectType, IntVector2 indexSize, IntVector2? parentIndex = null)
    {
        prefab = o;
        objectName = prefab.name.Replace("(Clone)", "");
        type = cellType;
        objectType = cellObjectType;
        size = indexSize;
        primaryIndex = parentIndex;
    }
}
