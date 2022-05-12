using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditWallController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Dropdown wallTypeDropdown;
    // Start is called before the first frame update
    public void WallChanged()
    {
        var wallType = wallTypeDropdown.options[wallTypeDropdown.value].text;
        Sprite sprite = Resources.Load<Sprite>("images/room/" + wallType + "/baseWalls");
        spriteRenderer.sprite = sprite;
    }
}
