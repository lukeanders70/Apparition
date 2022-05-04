using System.IO;
using UnityEngine;

namespace StaticDungeon
{
    public static class RoomLoader
    {
        public static void Save(EditGridData gridToSave, string wallType, bool lockIn, string fileName)
        {
            Debug.Log("Saving File");
            var saveRoom = new SaveRoom();

            saveRoom.Cells = gridToSave.Cells;
            saveRoom.wallType = wallType;
            saveRoom.lockIn = lockIn;

            string json = JsonUtility.ToJson(saveRoom);

            File.WriteAllText(Application.dataPath + "/Resources/rooms/" + fileName + ".txt", json);
            Debug.Log("Save Complete");
        }

        public static SaveRoom LoadSaveRoomData(string fileName)
        {
            Debug.Log("Loading File");
            var fileBytes = File.ReadAllBytes(Application.dataPath + "/Resources/rooms/" + fileName + ".txt");
            var saveRoomData = JsonUtility.FromJson<SaveRoom>(System.Text.Encoding.UTF8.GetString(fileBytes));
            return saveRoomData;
        }
    }
}
