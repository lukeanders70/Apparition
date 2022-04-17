using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditGridController : MonoBehaviour
{
    public RoomGrid roomGrid = new RoomGrid();

    public EditGridData SerlializeGrid()
    {
        List<GridCell> cellData = new List<GridCell>();
        for (int colIndex = 0; colIndex < roomGrid.objectLocations.GetLength(0);  colIndex++)
        {
            for (int rowIndex = 0; rowIndex < roomGrid.objectLocations.GetLength(1); rowIndex++)
            {
                GridCell cell = roomGrid.objectLocations[colIndex, rowIndex];
                if(cell != null && cell.type == GridCell.CellType.primary)
                {
                    cellData.Add(cell);
                }
            }
        }
        EditGridData data = new EditGridData();
        data.Cells = cellData;
        return data;
    }

    public void ClearGrid()
    {
        
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Destroy(go);
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(go);
        }
        roomGrid.clear();

    }


    public Vector2? AddObjectToScene(GameObject o, GridCell.CellObjectType oType, int xIndex, int yIndex)
    {

        var newObj = Object.Instantiate(o);
        newObj.transform.parent = transform;
        var newPosition = roomGrid.addObject(newObj, oType, xIndex, yIndex);
        if (newPosition != null)
        {
            newObj.transform.localPosition = new Vector3(newPosition.Value.x, newPosition.Value.y, 0);
        }
        Behaviour[] componenets = newObj.GetComponentsInChildren<Behaviour>();
        Debug.Log(componenets.Length);
        foreach(Behaviour component in componenets)
        {
            Debug.Log("Componenet " + component.GetType().ToString());
            if(component.name != "SpriteRenderer")
            {
                component.enabled = false;
            }
        }
        return newPosition;
    }
}

[System.Serializable]
public class EditGridData
{
    public List<GridCell> Cells;
}
