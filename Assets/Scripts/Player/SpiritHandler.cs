using UnityEngine;
using UnityEngine.InputSystem;


public class SpiritHandler : MonoBehaviour
{
    public GameObject spirit;
    public GameObject otherPlayer;

    private void Start()
    {
        if (spirit != null)
        {
            spirit.transform.SetParent(this.transform);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            AttemptSwap();
        }
    }

    public void AttemptSwap()
    {
        SpiritController sh = spirit.GetComponent<SpiritController>();
        if (spirit.transform.parent == this.transform && sh != null && otherPlayer != null)
        {
            sh.Swap(otherPlayer);
        }
    }
}
