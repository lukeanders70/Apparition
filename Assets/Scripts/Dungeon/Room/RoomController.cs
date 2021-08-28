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
        doorManager.SetupDoors(dungeon, indexPosition, doorSpawnProbabilityOverride);

        StaticDungeon.ObjectProbability<StaticDungeon.Room>[] roomProbs = Mathf.Abs(indexPosition.x) + Mathf.Abs(indexPosition.y) > 2 ? levelInfo.FarRooms : Mathf.Abs(indexPosition.x) + Mathf.Abs(indexPosition.y) > 1 ? levelInfo.MediumRooms : levelInfo.NearRooms;
        StaticDungeon.Room roomInfo = StaticDungeon.Utils.ChooseFromObjectProbability(roomProbs);

        SetRoomInfo(roomInfo);
    }

    public void SetRoomInfo(StaticDungeon.Room newRoomInfo)
    {
        StaticDungeon.SpawnConfig spawnConfigInfo = StaticDungeon.Utils.ChooseFromObjectProbability(newRoomInfo.SpawnConfigProbs);
        RoomGrid roomGrid = new RoomGrid();
        obsticleManager.ClearObjects();
        enemyManager.ClearObjects();
        obsticleManager.SetObsticles(roomGrid, spawnConfigInfo);
        enemyManager.SetEnemies(roomGrid, spawnConfigInfo);
    }

    public void ExitRoom()
    {
        enemyManager.ExitRoom();
        obsticleManager.ExitRoom();
    }

    public void EnterRoom()
    {
        enemyManager.SpawnObjects();
        obsticleManager.SpawnObjects();
    }
}
