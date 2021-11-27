using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid
{
    #nullable enable
    public static int cellSize = 8;
    public static (int, int) center = (13, 6);
    GridCell?[,] objectLocations = new GridCell[26, 13];

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
        return new IntVector2(
            Mathf.FloorToInt(distanceFromTopLeft.x),
            Mathf.FloorToInt(distanceFromTopLeft.y)
        );
    }

    public static Vector2 GetLocationFromCell(IntVector2 cellIndex)
    {
        return new Vector2(
            cellIndex.x - (center.Item1) + 0.5f,
            -(cellIndex.y - (center.Item2) + 0.5f)
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
                if(obj != null)
                {
                    if(obj.primaryIndex != null)
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
    public Vector2? addObject(GameObject o, int xIndex, int yIndex)
    {
        IntVector2 size = GetSize(o);
        if(isEmpty(new IntVector2(xIndex, yIndex), size))
        {
            objectLocations[xIndex, yIndex] = new GridCell(o, GridCell.CellType.primary, size);
            for (int i = xIndex; i < xIndex + size.x; i++)
            {
                for (int j = yIndex; j < yIndex + size.y; j++)
                {
                    if(!(i == xIndex && j == yIndex))
                    {
                        objectLocations[i, j] = new GridCell(o, GridCell.CellType.overflow, size, new IntVector2(xIndex, yIndex));
                    }
                }
            }
            return GetLocationCell(xIndex, yIndex, o);
        }
        return null;
    }

    private IntVector2? findPrimaryIndex(int xIndex, int yIndex)
    {
        if(objectLocations[xIndex, yIndex] == null)
        {
            return null;
        } else if(objectLocations[xIndex, yIndex]?.type == GridCell.CellType.primary)
        {
            return new IntVector2(xIndex, yIndex);
        } else if(objectLocations[xIndex, yIndex]?.primaryIndex != null)
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
        if(primaryIndex == null)
        {
            return null;
        }
        GridCell? primary = objectLocations[primaryIndex.x, primaryIndex.y];
        GameObject? o = primary?.prefab;
        for(int i = primaryIndex.x; i < primaryIndex.x + primary?.size.x; i++)
        {
            for (int j = primaryIndex.x; j < primaryIndex.y + primary?.size.y; j++)
            {
                objectLocations[i, j] = null;
            }
        }
        return o;
    }
    public bool isEmpty(IntVector2 index, IntVector2? size)
    {
        if(size == null)
        {
            size = new IntVector2(1, 1);
        }
        if (!IsInRange(index.x, index.y, size))
        {
            return false;
        }
        for(int i = index.x; i < index.x + size.x; i++)
        {
            for (int j = index.y; j < index.y + size.y; j++)
            {
                if(objectLocations[i, j] != null)
                {
                    return false;
                }
            }
        }
        return true;
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

class GridCell
{
    public enum CellType
    {
        primary, // top left cell containing object
        overflow, // cell an object overflows into because of its size
    }

    public GameObject prefab;
    public CellType type;
    public IntVector2 size;
    public IntVector2? primaryIndex = null;
    public GridCell(GameObject o, CellType cellType, IntVector2 indexSize, IntVector2? parentIndex = null)
    {
        prefab = o;
        type = cellType;
        size = indexSize;
        primaryIndex = parentIndex;
    }
}
