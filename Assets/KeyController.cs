using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private float timeSinceStart = 0;
    public float frequencySeconds;
    public float maxOffsetMagnitude;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        timeSinceStart = timeSinceStart + Time.deltaTime;
        var rads = (timeSinceStart / frequencySeconds) * (2 * Mathf.PI);
        var offset = maxOffsetMagnitude * Mathf.Sin(rads);
        transform.position = new Vector3(0, offset);
    }
}
