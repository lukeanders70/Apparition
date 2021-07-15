using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private int maxEnemies;

    [SerializeField]
    private int minEnemies;

    [SerializeField]
    private GameObject enemyPrefab;

    List<EnemyInfo> enemyPrefabs = new List<EnemyInfo>();
    List<GameObject> enemies = new List<GameObject>();

    public void SetEnemies(Vector2 roomIndexPosition, StaticDungeonInfo staticDungeonInfo)
    {
        int numEnemies = roomIndexPosition == Vector2.zero ? 0 : Random.Range(maxEnemies, minEnemies);
        for (int i = 0; i < numEnemies; i++)
        {

            enemyPrefabs.Add(new EnemyInfo(enemyPrefab, staticDungeonInfo.getEnemyPosition(numEnemies, i)));
        }

    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            GameObject newEnemy = Instantiate(
                enemyPrefabs[i].prefab,
                gameObject.transform,
                false
            );
            newEnemy.transform.localPosition = enemyPrefabs[i].spawnPosition;
            enemies.Add(newEnemy);
        }
    }

    public void ExitRoom()
    {
        if(enemies.Count == enemyPrefabs.Count)
        {
            List<EnemyInfo> newEnemyPrefabs = new List<EnemyInfo>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] != null)
                {
                    newEnemyPrefabs.Add(enemyPrefabs[i]);
                    Destroy(enemies[i]);
                }
            }
            enemyPrefabs = newEnemyPrefabs;
            enemies = new List<GameObject>();
        } else
        {
            Debug.LogError($"enemy count {enemies.Count} did not equal prefab count {enemyPrefabs.Count} in room");
        }
    }
}

class EnemyInfo
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    public EnemyInfo(GameObject pref, Vector3 sPosition)
    {
        prefab = pref;
        spawnPosition = sPosition;
    }
}
