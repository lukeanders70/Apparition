using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButtonController : MonoBehaviour
{
    private GameObject tilePrefab;
    private CurrentToolController toolController;
    private Sprite sprite;
    private GridCell.CellObjectType type;
    // Start is called before the first frame update
    public void Setup(GridCell.CellObjectType t, GameObject prefab)
    {
        type = t;
        tilePrefab = prefab;
        sprite = tilePrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = sprite;

        toolController = FindObjectOfType<CurrentToolController>();
    }

    public void SelectPrefab()
    {
        toolController.SetTool(type, sprite, tilePrefab);
    }
}
