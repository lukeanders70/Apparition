using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D self;
    [SerializeField]
    private Rigidbody2D toFollow;
    [SerializeField]
    private float forceMultiplier;

    private Vector2 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        self.position = toFollow.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lastPosition != null)
        {
            self.position = lastPosition;
        }
        lastPosition = toFollow.position;
        Vector2 direction = toFollow.position - self.position;
        self.AddForce(direction * forceMultiplier);
    }
}
