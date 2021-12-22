using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class BasicHealth : MonoBehaviour, Health
{
    public int health;
    public int maxHealth;

    public bool invicible = false;

    public Color deathColor;

    private ParticleSystem? deathParticals;

    public bool isDestroyed = false;

    private List<Action> deathCallbacks = new List<Action>();

    public void Awake()
    {
        health = maxHealth;
        deathParticals = Resources.Load<ParticleSystem>("prefabs/Effects/Destroy");
    }

    virtual public bool Damage(int damage)
    {
        if (!invicible)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Kill();
            }
            return true;
        }
        else
        {
            return false;
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

    protected void SpawnDeathParticles()
    {
        ParticleSystem? psInstance = Instantiate(deathParticals);
        if (psInstance != null)
        {
            psInstance.transform.position = transform.position;
            ParticleSystem.MainModule ma = psInstance.main;
            ma.startColor = deathColor;
        }
    }

    virtual public void Kill()
    {
        foreach (Action deathCallback in deathCallbacks)
        {
            deathCallback();
        }
        gameObject.tag = "Destroyed";
        SpawnDeathParticles();
        Destroy(gameObject);
    }

    public void AddDeathCallback(Action callback)
    {
        deathCallbacks.Add(callback);
    }
}