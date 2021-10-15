using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Health
{
    public bool Damage(int damage);

    public int GetHealth();

    public int GetMaxHealth();

    public void Heal(int amount);

    public void Kill();

    public void AddDeathCallback(System.Action callback);
}
