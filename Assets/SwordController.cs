using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public int damage = 1;
    public Animator animator;
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

    public void SetLeft()
    {
        transform.localPosition = new Vector3(-0.7f, transform.localPosition.y);
        animator.SetFloat("direction", -1.0f);
    }

    public void SetRight()
    {
        transform.localPosition = new Vector3(0.7f, transform.localPosition.y);
        animator.SetFloat("direction", 1.0f);
    }


    public void Attack()
    {
        animator.SetTrigger("attack");
    }
}
