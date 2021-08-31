using System;
using UnityEngine;
using System.Collections;

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

    private bool damagable = true;

    public bool Damage(int damage)
    {
        if(damagable)
        {
            health = Math.Max(0, health - 1);
            healthContainer.UpdateHealth(health);
            StartCoroutine(HitInvincible());
            if (health == 0)
            {
                Kill();
            }
            return true;
        }
        return false;
    }

    IEnumerator HitInvincible()
    {
        damagable = false;
        playerHandler.SetColor(new Color(1, 1, 1, 0.5f)); // slightly transparent
        yield return new WaitForSeconds(2);
        playerHandler.SetColor(new Color(1, 1, 1, 1)); // white
        damagable = true;
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
        GameObject transitions = GameObject.Find("Transitions");
        StartCoroutine(transitions.GetComponent<Transitions>().LevelTransition(
            () =>
            {
                Destroy(playerHandler.player1.gameObject);
                Destroy(playerHandler.player2.gameObject);
                gameOver.SetActive(true);
            }
        ));
    }
}
