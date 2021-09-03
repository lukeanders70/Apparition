using UnityEngine;
using UnityEngine.InputSystem;


public class SpiritHandler : MonoBehaviour
{
    public GameObject spirit;
    public GameObject otherPlayer;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spirit.transform.parent != transform)
        {
            spriteRenderer.material.SetFloat("_GrayscaleAmount", 1.0f);
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
        SpiritController sc = spirit.GetComponent<SpiritController>();
        if (spirit.transform.parent == this.transform && sc != null && otherPlayer != null)
        {
            RemoveSpirit();
            sc.Swap(otherPlayer);
        }
    }

    public void RemoveSpirit()
    {
        spriteRenderer.material.SetFloat("_GrayscaleAmount", 1.0f);
    }

    public void ReceiveSpirit()
    {
        spriteRenderer.material.SetFloat("_GrayscaleAmount", 0.0f);
    }
}
