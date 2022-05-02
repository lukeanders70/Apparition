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
    Text FileName;
    public void Save()
    {
        Debug.Log("Saving File");
        var saveRoom = new SaveRoom();

        var gridToSave = gridController.SerlializeGrid();
        var wallType = walTypeDropdown.options[walTypeDropdown.value].text;
        var isLockIn = lockInToggle.isOn;

        saveRoom.Cells = gridToSave.Cells;
        saveRoom.wallType = wallType;
        saveRoom.lockIn = isLockIn;

        string json = JsonUtility.ToJson(saveRoom);

        File.WriteAllText(Application.dataPath + "/Resources/rooms/" + FileName.text + ".txt", json);
        Debug.Log("Save Complete");
    }

    public void Load()
    {
        Debug.Log("Loading File");
        // Stream the file with a File Stream. (Note that File.Create() 'Creates' or 'Overwrites' a file.)
        var fileBytes = File.ReadAllBytes(Application.dataPath + "/Resources/rooms/" + FileName.text + ".txt");
        var gridToLoad = JsonUtility.FromJson<EditGridData>(System.Text.Encoding.UTF8.GetString(fileBytes));

        gridController.ClearGrid();
        foreach (GridCell cell in gridToLoad.Cells)
        {
            if (cell.type == GridCell.CellType.primary)
            {
                string foldername = cell.objectType == GridCell.CellObjectType.enemy ? "Enemies" : "Obstacles";
                GameObject prefab = Resources.Load<GameObject>("prefabs/" + foldername + "/" + cell.objectName);
                gridController.AddObjectToScene(prefab, cell.objectType, cell.primaryIndex.x, cell.primaryIndex.y);
            }
        }
        Debug.Log("Load Complete");
    }
}

[System.Serializable]
public class SaveRoom
{
    public List<GridCell> Cells;
    public bool lockIn;
    public string wallType;
}
