using UnityEngine;

public class IndividualPlayerHealth : MonoBehaviour, Health
{
    [SerializeField]
    private PlayerHealth healthDelegate;

    public void Damage(int damage)
    {
        healthDelegate.Damage(damage);
    }

    public int GetHealth()
    {
        return healthDelegate.GetHealth();
    }

    public int GetMaxHealth()
    {
        return healthDelegate.GetMaxHealth();
    }

    public void Heal(int amuount)
    {
        healthDelegate.Heal(amuount);
    }

    public void Kill()
    {
        healthDelegate.Kill();
    }
}
