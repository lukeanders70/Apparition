using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private float timeSinceStart = 0;
    public float frequencySeconds;
    public float maxOffsetMagnitude;
    [SerializeField]
    private ParticleSystem ps;

    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        timeSinceStart = timeSinceStart + Time.deltaTime;
        var rads = (timeSinceStart / frequencySeconds) * (2 * Mathf.PI);
        var offset = maxOffsetMagnitude * Mathf.Sin(rads);
        transform.position = originalPosition + new Vector3(0, offset);
    }

    virtual protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player")
        {
            InventoryController inventory = collidedObject.GetComponentInParent<InventoryController>();
            inventory.GiveKey();
            ParticleSystem hitParticals = Instantiate(ps);
            hitParticals.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
