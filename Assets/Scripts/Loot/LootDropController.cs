using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropController : MonoBehaviour
{
    [SerializeField]
    private GameObject lootPrefab;
    [SerializeField]
    private int min;
    [SerializeField]
    private int max;

    const float scatterVecoity = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        SetDeathCallback();
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
        for (int i = 0; i < prefabCount; i++)
        {
            GameObject lootInstance = Instantiate(lootPrefab);
            GameObject containingRoom = DungeonUtils.GetRoom(gameObject);
            if (lootInstance != null && containingRoom != null)
            {
                lootInstance.transform.position = transform.position;
                lootInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * scatterVecoity;
                lootInstance.transform.parent = containingRoom.transform;
            }
        }
    }
}
