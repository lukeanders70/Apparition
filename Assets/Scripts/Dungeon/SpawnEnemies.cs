using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private float respawnTime;

    [SerializeField]

    private float nextSpawnTime;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextSpawnTime)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        nextSpawnTime = Time.time + respawnTime;
        Vector3 randomDist = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        GameObject.Instantiate(EnemyPrefab, transform.position + randomDist, transform.rotation);
    }
}
