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

    public bool follow = false;

    private Vector2 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        if(toFollow != null)
        {
            self.position = toFollow.position;
        }
    }

    public void SetFollow(GameObject o)
    {
        if (o == null) { toFollow = null; follow = false; return; }
        var followRB = o.GetComponent<Rigidbody2D>();
        if(followRB == null)
        {
            Debug.LogError("Failed to Follow Object, no RigidBody2D found");
        } else
        {
            toFollow = followRB;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (follow && toFollow != null)
        {
            Vector2 direction = toFollow.position - self.position;
            self.AddForce(direction * forceMultiplier);
        } else
        {
            self.velocity = Vector2.zero;
        }
    }
}
