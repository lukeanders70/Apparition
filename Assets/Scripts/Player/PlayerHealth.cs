using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Health
{
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private PlayerHandler playerHandler;
    [SerializeField]
    private HealthContainerController healthContainer;
    public int health;
    public int maxHealth;

    public void Damage(int damage)
    {
        health = Math.Max(0, health - 1);
        healthContainer.UpdateHealth(health);
        if(health == 0)
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
        Destroy(playerHandler.player1.gameObject);
        Destroy(playerHandler.player2.gameObject);
        gameOver.SetActive(true);
    }
}
