using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Tool
{
    public void UseTool(IntVector2 position, RoomGrid roomGrid, GameObject roomGridGameObject);
    public Sprite GetSprite();
}

public class PrefabSetter : Tool
{
    private Sprite sprite;
    private GameObject Prefab;
    private GridCell.CellObjectType type;
    public PrefabSetter(GridCell.CellObjectType t, Sprite spi, GameObject pref)
    {
        type = t;
        sprite = spi;
        Prefab = pref;
    }

    public void UseTool(IntVector2 position, RoomGrid roomGrid, GameObject roomGridGameObject)
    {
        var newObj = Object.Instantiate(Prefab);
        newObj.transform.parent = roomGridGameObject.transform;
        var newPosition = roomGrid.addObject(newObj, type, position.x, position.y);
        newObj.transform.localPosition = new Vector3(newPosition.Value.x, newPosition.Value.y, 0);
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}

public class CellClearer : Tool
{
    private Sprite sprite;
    public CellClearer() {
        sprite = Resources.Load<Sprite>("images/UI/Icons/trash");
    }

    public void UseTool(IntVector2 position, RoomGrid roomGrid, GameObject roomGridGameObject)
    {
        GameObject removedObj = roomGrid.removeObject(position.x, position.y);
        if(removedObj != null)
        {
            Object.Destroy(removedObj);
        }
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}

public class CurrentToolController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image image;

    private Tool tool;
    public void SetTool(Tool t)
    {
        tool = t;
        image.sprite = t.GetSprite();
    }

    public void UseTool(IntVector2 position, RoomGrid roomGrid, GameObject roomGridGameObject)
    {
        if(tool != null)
        {
            tool.UseTool(position, roomGrid, roomGridGameObject);
        } else
        {
            Debug.Log("No Tool Selected");
        }
    }
}
