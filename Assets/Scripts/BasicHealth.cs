using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class BasicHealth : MonoBehaviour, Health
{
    public int health;
    public int maxHealth;

    public void Awake()
    {
        health = maxHealth;
    }

    virtual public void Damage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            Kill();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void Heal(int amount)
    {
        Mathf.Min(health + amount, maxHealth);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}