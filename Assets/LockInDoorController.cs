using UnityEngine;

public class LockInDoorController : DoorController
{
    [SerializeField]
    public Animator animator;

    private EnemyManager roomEnemyManager;

    private void Start()
    {
        roomEnemyManager = transform.parent.gameObject.GetComponent<EnemyManager>();
    }

    private void OnEnemyKilled(object source, System.EventArgs eventArgs)
    {
        if (roomEnemyManager.ObjectCount() == 0) // final enemy will not be null yet, so total count will show as 1
            SetOpen();
    }
    override public void RoomEntered()
    {
        if(roomEnemyManager.objects.Count != 0)
        {
            SetClosed();
            roomEnemyManager.EnemyKilled += OnEnemyKilled;
        }
    }

    private void SetClosed()
    {
        state = DoorState.Closed;
        animator.SetBool("doorClosed", true);
    }

    private void SetOpen()
    {
        state = DoorState.Open;
        animator.SetBool("doorClosed", false);
    }
}
