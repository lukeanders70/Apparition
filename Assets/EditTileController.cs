using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EditTileController : MonoBehaviour
{
    // Start is called before the first frame update
    public IntVector2 index;
    public SpriteRenderer spriteRenderer;

    private Color mouseOverColor = new Color(1, 1, 1, 0.3f);
    private Color defaultColor;

    private RoomGrid roomGrid;
    private CurrentToolController currentTool;
    private GameObject uiGrid;
    void Start()
    {
        var yIndex = getNumberFromName(gameObject.transform.parent.gameObject.name);
        var xIndex = getNumberFromName(gameObject.name);
        index = new IntVector2(xIndex, yIndex);

        defaultColor = spriteRenderer.color;

        var editGridController = GetComponentInParent<EditGridController>();
        uiGrid = editGridController.gameObject;
        roomGrid = editGridController.roomGrid;
        currentTool = FindObjectOfType<CurrentToolController>();
    }

    void OnMouseOver()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        spriteRenderer.color = mouseOverColor;
    }

    void OnMouseExit()
    {
        // Reset the color of the GameObject back to normal
        spriteRenderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        if (currentTool.Prefab != null)
        {
            Debug.Log(index);
            var newObj = Instantiate(currentTool.Prefab);
            newObj.transform.parent = uiGrid.transform;
            var newPosition = roomGrid.addObject(newObj, currentTool.type, index.x, index.y);
            newObj.transform.localPosition = new Vector3(newPosition.Value.x, newPosition.Value.y, 0);
        }
    }

    private int getNumberFromName(string name)
    {
        var resultString = Regex.Match(name, "([0-9]+)", RegexOptions.ExplicitCapture).Value;
        if(resultString == null || resultString == "")
        {
            return 0;
        }
        var resultNum = int.Parse(resultString);
        return resultNum;
    }
}
