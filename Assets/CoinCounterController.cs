using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounterController : MonoBehaviour
{
    [SerializeField]
    private Text counterText;
    
    public void UpdateCount(int newCount)
    {
        counterText.text = newCount.ToString();
    }
}
