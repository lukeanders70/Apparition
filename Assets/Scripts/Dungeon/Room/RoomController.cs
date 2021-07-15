using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
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
        doorManager.SetupDoors(dungeon, indexPosition, doorSpawnProbabilityOverride);
        enemyManager.SetEnemies(indexPosition, staticDungeonInfo);
    }

    public void ExitRoom()
    {
        enemyManager.ExitRoom();
    }

    public void EnterRoom()
    {
        enemyManager.SpawnEnemies();
    }
}
