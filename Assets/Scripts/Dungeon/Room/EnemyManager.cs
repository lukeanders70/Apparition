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

    List<GameObject> enemyPrefabs = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();

    public void SetEnemies()
    {
        int numEnemies = Random.Range(maxEnemies, minEnemies);
        for (int i = 0; i < numEnemies; i++)
        {
            enemyPrefabs.Add(enemyPrefab);
        }

    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            GameObject newEnemy = Instantiate(
                enemyPrefabs[i],
                gameObject.transform,
                false
            );
            newEnemy.transform.localPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            enemies.Add(newEnemy);
        }
    }

    public void ExitRoom()
    {
        if(enemies.Count == enemyPrefabs.Count)
        {
            List<GameObject> newEnemyPrefabs = new List<GameObject>();
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
