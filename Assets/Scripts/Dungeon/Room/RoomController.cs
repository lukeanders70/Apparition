using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
    [SerializeField]
    private DoorManager doorManager;

    public void SetupRoom(
        Dictionary<Vector2, GameObject> dungeon,
        Vector2 indexPosition,
        float? doorSpawnProbabilityOverride
    )
    {
        doorManager.SetupDoors(dungeon, indexPosition, doorSpawnProbabilityOverride);
        enemyManager.SetEnemies(indexPosition);
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
