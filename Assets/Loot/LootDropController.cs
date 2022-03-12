using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropController : MonoBehaviour
{
    [SerializeField]
    private GameObject lootPrefab;
    [SerializeField]
    private bool dropOnDeath = true;
    [SerializeField]
    private int min;
    [SerializeField]
    private int max;
    [SerializeField]
    private Vector2 spawnOffset = Vector2.zero;

    const float scatterVecoity = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(dropOnDeath)
        {
            SetDeathCallback();
        }
    }

    private void SetDeathCallback()
    {
        Health hComp = gameObject.GetComponent<Health>();
        if (hComp != null)
        {
            hComp.AddDeathCallback(AttemptLootDrop);
        }
    }

    public void AttemptLootDrop()
    {
        int prefabCount = Random.Range(min, max);
        GameObject containingRoom = DungeonUtils.GetRoom(gameObject);
        for (int i = 0; i < prefabCount; i++)
        {
            GameObject lootInstance = Instantiate(lootPrefab);
            if (lootInstance != null && containingRoom != null)
            {
                lootInstance.transform.position = (Vector2)transform.position + spawnOffset;
                lootInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * scatterVecoity;
                lootInstance.transform.parent = containingRoom.transform;
            }
        }
    }
}
