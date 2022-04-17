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
    Text FileName;
    public void Save()
    {
        Debug.Log("Saving File");
        var gridToSave = gridController.SerlializeGrid();

        string json = JsonUtility.ToJson(gridToSave);

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
                cell.prefab = prefab;
                gridController.AddObjectToScene(prefab, cell.objectType, cell.primaryIndex.x, cell.primaryIndex.y);
            }
        }
        Debug.Log("Load Complete");
    }
}
