using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private int maxEnemies;

    [SerializeField]
    private int minEnemies;

    List<EnemyInfo> enemyPrefabs = new List<EnemyInfo>();
    List<GameObject> enemies = new List<GameObject>();

    public void SetEnemies(Vector2 roomIndexPosition, StaticDungeonInfo staticDungeonInfo, RoomType roomType)
    {
        int numEnemies = roomIndexPosition == Vector2.zero ? 0 : Random.Range(maxEnemies, minEnemies);
        for (int i = 0; i < numEnemies; i++)
        {
            var prefab = getEnemyPrefab(roomType);
            if(prefab != null)
            {
                enemyPrefabs.Add(new EnemyInfo(prefab, getEnemyPosition(staticDungeonInfo.enemyConfigurations, numEnemies, i)));
            }
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

    private Vector2 getEnemyPosition(EnemyConfiguration[] enemyConfigurations, int numEnemies, int enemyIndex)
    {
        if (numEnemies - 1 < enemyConfigurations.Length)
        {
            if (enemyIndex < enemyConfigurations[numEnemies - 1].positions.Length)
            {
                return enemyConfigurations[numEnemies - 1].positions[enemyIndex].ToVector();
            }
        }
        return new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
    }

    #nullable enable
    private GameObject? getEnemyPrefab(RoomType roomType)
    {
        float r = Random.Range(0f, 1.0f);
        var sum = 0f;
        foreach (EnemyProbability ep in roomType.enemyProbabilities)
        {
            sum += ep.probability;
            if(sum > r)
            {
                return Resources.Load<GameObject>("prefabs/Enemies/" + ep.name);
            }
        }
        return null;
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
