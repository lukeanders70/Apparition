using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public Animator animator;
    private int damage = 1;
    public float speed = 10;
    public Vector2 direction;

    public void Start()
    {
        // Set the effect to be destroyed after animation is complete
        var animationCallback = animator.GetBehaviour<AnimationCallback>();
        Debug.Log(animationCallback);
        animationCallback.exitCallbacks.Add((Animator a, AnimatorStateInfo stateInfo, int layerIndex) => {
            Destroy(gameObject);
        });
    }

    public void Update()
    {
        if(direction != null)
        {
            Debug.Log("Direction not null");
            transform.position = (Vector2)transform.position + (direction * (speed * Time.deltaTime));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.tag == "Player")
        {
            Health colliderHealth = collision.GetComponent<Health>();
            if (colliderHealth != null)
            {
                colliderHealth.Damage(damage);
            }
        }
    }
}
