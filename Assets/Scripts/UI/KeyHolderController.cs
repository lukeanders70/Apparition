using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyHolderController : MonoBehaviour
{
    [SerializeField]
    private GameObject keyImage;

    public void GiveKey()
    {
        keyImage.SetActive(true);
    }

    public void RemoveKey()
    {
        keyImage.SetActive(false);
    }
}
