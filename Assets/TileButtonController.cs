using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButtonController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image image;
    private CurrentToolController currentToolController;
    private Tool tool;  
    public void Setup(Tool t)
    {
        currentToolController = FindObjectOfType<CurrentToolController>();
        tool = t;
        image.sprite = tool.GetSprite();
    }

    public void SelectPrefab()
    {
        currentToolController.SetTool(tool);
    }
}
