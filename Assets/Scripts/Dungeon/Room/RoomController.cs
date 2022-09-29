using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
    [SerializeField]
    private ObsticleManager obsticleManager;
    [SerializeField]
    private DoorManager doorManager;
    [SerializeField]
    private GameObject baseWalls;

    public RoomGrid roomGrid;

    public StaticDungeon.Room roomInfo;

    public Vector2 position;

    /**
     * Call when a room is first created
     */ 
    public void SetupRoom(
        Dictionary<Vector2, GameObject> dungeon,
        Vector2 indexPosition,
        StaticDungeon.Level levelInfo,
        float? doorSpawnProbabilityOverride
    )
    {
        var manDistanceFromStart = Mathf.Abs(indexPosition.x) + Mathf.Abs(indexPosition.y);

        StaticDungeon.ObjectProbability<StaticDungeon.Room>[] roomProbs =
            manDistanceFromStart > 4 ?
                levelInfo.FarRooms :
                manDistanceFromStart > 2 ?
                    levelInfo.MediumRooms :
                    levelInfo.NearRooms;

        roomInfo = StaticDungeon.Utils.ChooseFromObjectProbability(roomProbs);
        position = indexPosition;

        doorManager.SetupDoors(dungeon, indexPosition, roomInfo, doorSpawnProbabilityOverride);

        SetRoomInfo(dungeon, indexPosition, roomInfo);
    }

    public void SetRoomInfo(Dictionary<Vector2, GameObject> dungeon, Vector2 indexPosition, StaticDungeon.Room newRoomInfo)
    {
        StaticDungeon.SpawnConfig spawnConfigInfo = StaticDungeon.Utils.ChooseFromObjectProbability(newRoomInfo.SpawnConfigProbs);
        roomGrid = new RoomGrid();

        obsticleManager.ClearObjects();
        enemyManager.ClearObjects();

        obsticleManager.SetObsticles(roomGrid, spawnConfigInfo);
        enemyManager.SetEnemies(roomGrid, spawnConfigInfo);

        doorManager.ResetDoorwayWallSprites(newRoomInfo.WallType);

        setWallSprite(newRoomInfo.WallType);

        if(newRoomInfo.isBossRoom)
        {
            doorManager.SetBossRoomNeighbors(dungeon, indexPosition);
        }

        roomInfo = newRoomInfo;
    }

    private void setWallSprite(string wallType)
    {
        var path = "images/Room/" + wallType + "/" + "baseWalls";
        Sprite doorwayWallSprite = Resources.Load<Sprite>(path);
        if (doorwayWallSprite != null)
        {
            baseWalls.GetComponent<SpriteRenderer>().sprite = doorwayWallSprite;
        }
        else
        {
            Debug.LogError("Could not find doorway wall at " + path);
        }
    }

    public void ExitRoom()
    {
        enemyManager.ExitRoom();
        obsticleManager.ExitRoom();
    }

    public void EnterRoom()
    {
        enemyManager.SpawnObjects(roomGrid);
        obsticleManager.SpawnObjects(roomGrid);
        doorManager.RoomEntered();
    }
}
