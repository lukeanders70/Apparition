using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    public void Damage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if( health == 0 )
        {
            Destroy(gameObject);
        }
    }
}
