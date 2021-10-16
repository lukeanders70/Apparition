using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private CoinCounterController CoinCounter;
    public int numCoins = 0;
    public int maxCoins = 999;
    public void AddCoins(int amount = 1)
    {
        numCoins += amount;
        numCoins = Mathf.Min(maxCoins, numCoins);
        CoinCounter.UpdateCount(numCoins);
    }
}
