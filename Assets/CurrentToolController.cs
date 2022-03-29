using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentToolController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image image;
    public GameObject Prefab;
    public GridCell.CellObjectType type;
    public void SetTool(GridCell.CellObjectType t, Sprite spi, GameObject pref)
    {
        type = t;
        image.sprite = spi;
        Prefab = pref;
    }
}
