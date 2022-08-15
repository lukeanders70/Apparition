using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private CoinCounterController CoinCounter;
    [SerializeField]
    private KeyHolderController KeyHolder;
    public int maxCoins = 999;
    public void AddCoins(int amount = 1)
    {
        PlayerStateInfo.coins += amount;
        PlayerStateInfo.coins = Mathf.Min(maxCoins, PlayerStateInfo.coins);
        CoinCounter.UpdateCount(PlayerStateInfo.coins);
    }

    public bool RemoveCoins(int amount = 1)
    {
        if(PlayerStateInfo.coins >= amount)
        {
            PlayerStateInfo.coins -= amount;
            CoinCounter.UpdateCount(PlayerStateInfo.coins);
            return true;
        }
        return false;
    }

    public void GiveKey()
    {
        KeyHolder.GiveKey();
    }

    public void RemoveKey()
    {
        KeyHolder.RemoveKey();
    }
}
