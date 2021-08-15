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
        StaticDungeonInfo staticDungeonInfo,
        float? doorSpawnProbabilityOverride
    )
    {
        RoomType roomType = staticDungeonInfo.roomTypes[Random.Range(0, staticDungeonInfo.roomTypes.Length)];
        RoomGrid roomGrid = new RoomGrid();

        doorManager.SetupDoors(dungeon, indexPosition, doorSpawnProbabilityOverride);
        obsticleManager.SetObsticles(indexPosition, staticDungeonInfo, roomGrid, roomType);
        enemyManager.SetEnemies(indexPosition, staticDungeonInfo, roomGrid, roomType);
    }

    public void ExitRoom()
    {
        enemyManager.ExitRoom();
        obsticleManager.ExitRoom();
    }

    public void EnterRoom()
    {
        enemyManager.SpawnEnemies();
        obsticleManager.SpawnObjects();
    }
}
