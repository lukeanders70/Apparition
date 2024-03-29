using UnityEngine;
using System.Collections.Generic;

public class IndividualPlayerHealth : MonoBehaviour, Health
{
    [SerializeField]
    private PlayerHealth healthDelegate;

    private ParticleSystem damageParticals;

    private List<System.Action> deathCallbacks = new List<System.Action>();

    public void Awake()
    {
        damageParticals = Resources.Load<ParticleSystem>("prefabs/Effects/Hit");
    }
    public bool Damage(int damage)
    {
        bool didDamage = healthDelegate.Damage(damage);
        if (didDamage)
        {
            ParticleSystem psInstance = Instantiate(damageParticals);
            ParticleSystem.MainModule ma = psInstance.main;
            ma.startColor = new Color(1.0f, 0.2f, 0.2f);
            psInstance.transform.position = transform.position;
        }
        return didDamage;
    }

    public int GetHealth()
    {
        return healthDelegate.GetHealth();
    }

    public int GetMaxHealth()
    {
        return healthDelegate.GetMaxHealth();
    }

    public void Heal(int amount)
    {
        healthDelegate.Heal(amount);
    }

    public void Kill()
    {
        healthDelegate.Kill();
    }

    public void AddDeathCallback(System.Action callback)
    {
        deathCallbacks.Add(callback);
    }
}
