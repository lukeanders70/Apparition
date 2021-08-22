using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid
{
    private (int, int) center = (13, 6);
    GridCell[,] objectLocations = new GridCell[26, 13];

    public Vector2 GetLocationCell(int xIndex, int yIndex, GameObject o)
    {
        (int, int) size = GetSize(o);
        Vector2 offset = GetOffset(o);
        float xVal = ((xIndex - center.Item1) + ((float) size.Item1 / 2.0f)) - offset.x;
        float yVal = -(((yIndex - center.Item2) + ((float) size.Item2 / 2.0f)) + offset.y);
        return new Vector2(xVal, yVal);
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

    public Vector2? addObject(GameObject o, int xIndex, int yIndex)
    {
        (int, int) size = GetSize(o);
        if(isEmpty(xIndex, yIndex, size))
        {
            objectLocations[xIndex, yIndex] = new GridCell(o, GridCell.CellType.primary, size);
            for (int i = xIndex; i < xIndex + size.Item1; i++)
            {
                for (int j = yIndex; j < yIndex + size.Item2; j++)
                {
                    if(!(i == xIndex && j == yIndex))
                    {
                        objectLocations[i, j] = new GridCell(o, GridCell.CellType.overflow, size, (xIndex, yIndex));
                    }
                }
            }
            return GetLocationCell(xIndex, yIndex, o);
        }
        return null;
    }

    private (int, int)? findPrimaryIndex(int xIndex, int yIndex)
    {
        if(objectLocations[xIndex, yIndex] == null)
        {
            return null;
        } else if(objectLocations[xIndex, yIndex].type == GridCell.CellType.primary)
        {
            return (xIndex, yIndex);
        } else if(objectLocations[xIndex, yIndex].primaryIndex != null)
        {
            return ((int, int)) objectLocations[xIndex, yIndex].primaryIndex;
        } else
        {
            return null;
        }
    }

    public GameObject? removeObject(int xIndex, int yIndex)
    {
        (int, int)? primaryIndexNullable = findPrimaryIndex(xIndex, yIndex);
        if(primaryIndexNullable == null)
        {
            return null;
        }
        (int, int) primaryIndex = ((int, int)) primaryIndexNullable;
        GridCell primary = objectLocations[primaryIndex.Item1, primaryIndex.Item2];
        GameObject o = primary.prefab;
        for(int i = primaryIndex.Item1; i < primaryIndex.Item1 + primary.size.Item1; i++)
        {
            for (int j = primaryIndex.Item1; j < primaryIndex.Item2 + primary.size.Item2; j++)
            {
                GridCell currentCell = objectLocations[i, j];
                objectLocations[i, j] = null;
            }
        }
        return o;
    }

    public bool isEmpty(int xIndex, int yIndex, (int, int) size)
    {
        if (!IsInRange(xIndex, yIndex, size))
        {
            return false;
        }
        for(int i = xIndex; i < xIndex + size.Item1; i++)
        {
            for (int j = yIndex; j < yIndex + size.Item2; j++)
            {
                if(objectLocations[i, j] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsInRange(int xIndex, int yIndex, (int, int) size)
    {
        return !(
            xIndex < 0 ||
            yIndex < 0 ||
            xIndex + size.Item1 > objectLocations.GetLength(0) || 
            yIndex + size.Item2 > objectLocations.GetLength(1)
            );
    }

    private (int, int) GetSize(GameObject o)
    {
        BoxCollider2D oCollider;
        bool hasCollider = o.TryGetComponent(out oCollider);
        if (hasCollider)
        {
            return ((int) System.Math.Ceiling(oCollider.size.x), (int) System.Math.Ceiling(oCollider.size.y));
        } else
        {
            return (1, 1);
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
    public (int, int) size;
    public (int, int)? primaryIndex = null;
    public GridCell(GameObject o, CellType cellType, (int, int) indexSize, (int, int)? parentIndex = null)
    {
        prefab = o;
        type = cellType;
        size = indexSize;
        primaryIndex = parentIndex;
    }
}
