using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private CoinCounterController CoinCounter;
    public int maxCoins = 999;
    public void AddCoins(int amount = 1)
    {
        PlayerStateInfo.coins += amount;
        PlayerStateInfo.coins = Mathf.Min(maxCoins, PlayerStateInfo.coins);
        CoinCounter.UpdateCount(PlayerStateInfo.coins);
    }
}
