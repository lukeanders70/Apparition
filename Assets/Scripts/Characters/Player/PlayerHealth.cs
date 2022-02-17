using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour, Health
{
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private PlayerHandler playerHandler;
    [SerializeField]
    private HealthContainerController healthContainer;
    public int startingHealth;

    private List<Action> deathCallbacks = new List<Action>();

    private bool damagable = true;

    void Start()
    {
        if(PlayerStateInfo.health == 0)
        {
            PlayerStateInfo.health = startingHealth;
            healthContainer.UpdateHealth(PlayerStateInfo.health);
        }
    }

    public bool Damage(int damage)
    {
        if(damagable)
        {
            PlayerStateInfo.health = Math.Max(0, PlayerStateInfo.health - 1);
            healthContainer.UpdateHealth(PlayerStateInfo.health);
            StartCoroutine(HitInvincible());
            if (PlayerStateInfo.health == 0)
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
        return PlayerStateInfo.health;
    }

    public int GetMaxHealth()
    {
        return startingHealth;
    }

    public void Heal(int amount)
    {
        PlayerStateInfo.health = PlayerStateInfo.health + amount;
        if(PlayerStateInfo.health > startingHealth)
        {
            startingHealth = PlayerStateInfo.health;
        }
        healthContainer.UpdateHealth(PlayerStateInfo.health);
    }

    public void Kill()
    {
        var transitions = GameObject.Find("Transitions");
        var levelTransition = transitions.GetComponent<Transitions>().LevelTransition();
        levelTransition.AddLoadingCallback((Animator a, AnimatorStateInfo stateInfo, int layerIndex) =>
        {
            Destroy(playerHandler.player1.gameObject);
            Destroy(playerHandler.player2.gameObject);
            gameOver.SetActive(true);
        });
        levelTransition.StartTransition();
    }

    public void AddDeathCallback(Action callback)
    {
        deathCallbacks.Add(callback);
    }
}
