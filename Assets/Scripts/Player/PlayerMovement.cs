using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField]
    private float speed;
    private Vector2 movement;
    private Vector3 lastMovement = new Vector3(0,-1,0);

    [SerializeField]
    private GameObject interactionPoint;
    private Quaternion rotateClockwise = Quaternion.Euler(0, 0, 90);

    [SerializeField]
    private Animator animator;

    #nullable enable
    private PlayerMoveCallback? moveCallback;
    private bool forceTranslate = false;
    private Vector2 forcedTranslatePos = new Vector2(0, 0);
    private float forcedTranslateSpeed = 0;

    private bool CanMove()
    {
        return gameObject.GetComponent<SpiritHandler>().spirit.transform.parent == this.transform;
    }

    private void Update()
    {
        if (forceTranslate && forcedTranslatePos != null && forcedTranslateSpeed > 0)
        {
            if(Vector2.Distance(forcedTranslatePos, transform.position) > 0.03)
            {
                transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y), forcedTranslatePos, forcedTranslateSpeed * Time.deltaTime);
            } else
            {
                EndForcedTranslate();
            }

        } else if(CanMove())
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
            if(!(movement.x == 0 && movement.y == 0))
            {
                lastMovement = movement;
                animator.SetFloat("LastHorizontal", lastMovement.x);
                animator.SetFloat("LastVertical", lastMovement.y);
            }
            animator.SetFloat("HorizontalVelocity", movement.x);
            animator.SetFloat("VerticalVelocity", movement.y);
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }
        animator.SetBool("IsWalking", movement.sqrMagnitude > 0.01);
        interactionPoint.transform.position = transform.position + lastMovement;
    }

    void FixedUpdate()
    {
       // rb.AddForce(movement * speed * Time.deltaTime);
       if(!forceTranslate)
        {
            rb.velocity = movement * speed;
        }
    }

    public void ForceTranslate(Vector2 newPosition, float speed, PlayerMoveCallback? callback)
    {
        forcedTranslatePos = newPosition;
        forcedTranslateSpeed = speed;
        forceTranslate = true;
        rb.isKinematic = true;
        moveCallback = callback;
    }

    public void teleportTranslate(Vector2 newPosition)
    {
        rb.isKinematic = true;
        transform.position = newPosition;
        rb.isKinematic = false;
    }
    private void EndForcedTranslate()
    {
        forceTranslate = false;
        forcedTranslatePos = new Vector2(0, 0);
        forcedTranslateSpeed = 0;
        rb.isKinematic = false;

        if(moveCallback != null) { moveCallback(); }
    }
}

public delegate void PlayerMoveCallback();
