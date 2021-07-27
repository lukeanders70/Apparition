using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class BasicHealth : MonoBehaviour, Health
{
    public int health;
    public int maxHealth;

    public Color deathColor;

    private ParticleSystem? deathParticals;

    public void Awake()
    {
        health = maxHealth;
        deathParticals = Resources.Load<ParticleSystem>("prefabs/Effects/Destroy");
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
        ParticleSystem? psInstance = Instantiate(deathParticals);
        if (psInstance != null)
        {
            psInstance.transform.position = transform.position;
            ParticleSystem.MainModule ma = psInstance.main;
            ma.startColor = deathColor;
        }
        Destroy(gameObject);
    }
}