using System.IO;
using UnityEngine;

namespace StaticDungeon
{
    public static class RoomLoader
    {
        public static void Save(EditGridController gridController, string wallType, bool lockIn, string fileName)
        {
            Debug.Log("Saving File");
            var saveRoom = new SaveRoom();

            var gridToSave = gridController.SerlializeGrid();

            saveRoom.Cells = gridToSave.Cells;
            saveRoom.wallType = wallType;
            saveRoom.lockIn = lockIn;

            string json = JsonUtility.ToJson(saveRoom);

            File.WriteAllText(Application.dataPath + "/Resources/rooms/" + fileName + ".txt", json);
            Debug.Log("Save Complete");
        }

        public static void LoadToEditGrid(EditGridController gridController, string fileName)
        {
            Debug.Log("Loading File");
            // Stream the file with a File Stream. (Note that File.Create() 'Creates' or 'Overwrites' a file.)
            var fileBytes = File.ReadAllBytes(Application.dataPath + "/Resources/rooms/" + fileName + ".txt");
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

        public static void LoadToRoom(string fileName)
        {

        }
    }
}
