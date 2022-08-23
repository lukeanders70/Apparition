using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyHolderController : MonoBehaviour
{
    [SerializeField]
    private GameObject keyImage;
    private bool hasKey = false;

    public void GiveKey()
    {
        hasKey = true;
        keyImage.SetActive(true);
    }

    public bool RemoveKey()
    {
        var didHaveKey = hasKey;
        hasKey = false;
        keyImage.SetActive(false);
        return didHaveKey;
    }
}
