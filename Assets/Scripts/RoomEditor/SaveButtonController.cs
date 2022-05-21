using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonController : MonoBehaviour
{
    [SerializeField]
    EditGridController gridController;
    [SerializeField]
    Dropdown walTypeDropdown;
    [SerializeField]
    Toggle lockInToggle;

    [SerializeField]
    InputField FileName;
    public void Save()
    {
        var gridToSave = gridController.SerlializeGrid();
        var wallType = walTypeDropdown.options[walTypeDropdown.value].text;
        var isLockIn = lockInToggle.isOn;

        StaticDungeon.RoomLoader.Save(gridToSave, wallType, isLockIn, FileName.text);
    }

    public void Load()
    {
        var saveRoomData = StaticDungeon.RoomLoader.LoadSaveRoomData(FileName.text);

        gridController.ClearGrid();
        foreach (GridCell cell in saveRoomData.Cells)
        {
            if (cell.type == GridCell.CellType.primary)
            {
                string foldername = cell.objectType == GridCell.CellObjectType.enemy ? "Enemies" : "Obstacles";
                GameObject prefab = Resources.Load<GameObject>("prefabs/" + foldername + "/" + cell.objectName);
                gridController.AddObjectToScene(prefab, cell.objectType, cell.primaryIndex.x, cell.primaryIndex.y);
            }
        }
        Debug.Log(saveRoomData.lockIn);
        Debug.Log(lockInToggle);
        lockInToggle.isOn = saveRoomData.lockIn;
        walTypeDropdown.value = findDropDownIndexFromString(saveRoomData.wallType);
        Debug.Log("Load Complete");
    }

    private int findDropDownIndexFromString(string value)
    {
        var index = 0;
        foreach (Dropdown.OptionData option in walTypeDropdown.options)
        {
            if(option.text == value)
            {
                return index;
            }
            index += 1;
        }
        return index;
    }
}

[System.Serializable]
public class SaveRoom
{
    public List<GridCell> Cells;
    public bool lockIn;
    public string wallType;
}
