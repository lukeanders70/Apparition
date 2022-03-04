using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDispenseController : MonoBehaviour
{
    [SerializeField]
    private GameObject textCanvas;
    [SerializeField]
    private float interactDistance;
    [SerializeField]
    private int coinsLeft;

    public bool containsHeart = true;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem ps;

    private void Update()
    {
        if(containsHeart)
        {
            var closestPlayer = AIHelpers.GetClosestPlayer(transform.position);

            if (Vector2.Distance(closestPlayer.transform.position, transform.position) < interactDistance)
            {
                textCanvas.SetActive(true);
            }
            else
            {
                textCanvas.SetActive(false);
            }

            if (textCanvas.activeSelf)
            {
                if (Input.GetKeyDown("e"))
                {
                    var inventory = closestPlayer.GetComponentInParent<InventoryController>();
                    DepositCoin(inventory);
                }
            }
        }
    }

    private void DepositCoin(InventoryController inventory)
    {
        var success = inventory.RemoveCoins();
        if(success)
        {
            coinsLeft -= 1;
        }
        if(coinsLeft <= 0)
        {
            AwardHeart(inventory);
        }
    }

    private void AwardHeart(MonoBehaviour inventory)
    {
        SetInactive();
        inventory.gameObject.GetComponent<Health>().Heal(1);
    }

    private void SetInactive()
    {
        animator.SetBool("isEmpty", true);
        containsHeart = false;
        textCanvas.SetActive(false);
        ps.Stop();
    }
}
