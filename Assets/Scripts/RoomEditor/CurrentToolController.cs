using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Tool
{
    public void UseTool(IntVector2 position, RoomGrid roomGrid, EditGridController roomGridGameObject);
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

    public void UseTool(IntVector2 position, RoomGrid roomGrid, EditGridController roomGridGameObject)
    {
        roomGridGameObject.AddObjectToScene(Prefab, type, position.x, position.y);

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

    public void UseTool(IntVector2 position, RoomGrid roomGrid, EditGridController roomGridGameObject)
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

    public void UseTool(IntVector2 position, RoomGrid roomGrid, EditGridController roomGridGameObject)
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
