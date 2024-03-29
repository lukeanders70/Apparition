using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestController : MonoBehaviour
{
    [SerializeField]
    public Animator lidAnimator;
    [SerializeField]
    private float interactDistance;
    [SerializeField]
    private LootDropController lootController;
    private EnemyManager roomEnemyManager;
    private RoomSaveState roomSave;

    private bool isOpen = false;

    private void Start()
    {
        roomEnemyManager = transform.parent.gameObject.GetComponent<EnemyManager>();
        roomSave = GetComponentInParent<RoomSaveState>();
        var alreadyOpenedNullable = roomSave.GetBool("TreasureChestOpened");
        if (alreadyOpenedNullable is bool alreadyOpened && alreadyOpened)
        {
            isOpen = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldOpen())
        {
            Open();
        }
    }

    private bool ShouldOpen()
    {
        var closestPlayer = AIHelpers.GetClosestPlayer(transform.position);

        return !isOpen &&
            Vector2.Distance(closestPlayer.transform.position, transform.position) < interactDistance &&
            (!roomEnemyManager || roomEnemyManager.ObjectCount() == 0);
    }

    private void DropLoot()
    {
        lootController.AttemptLootDrop();
    }

    private void Open()
    {
        roomSave.AddBool("TreasureChestOpened", true);
        lidAnimator.SetBool("lift-lid", true);
        isOpen = true;
        Invoke("DropLoot", 1.0f);
    }

}
