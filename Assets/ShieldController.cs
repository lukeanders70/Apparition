using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public KnightAI ParentKnight;
    [SerializeField]
    private float KnightDistance;
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Vector3 KnightDisOffset;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float flinchDuration;

    private Color startcolor;

    [SerializeField]
    private float rotAngleDeg = 270;

    private void Start()
    {
        startcolor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        var parentPos = ParentKnight.transform.position;
        rotAngleDeg = (rotAngleDeg + (rotSpeed * Time.deltaTime)) % 360;
        animator.SetFloat("angle", rotAngleDeg);
        transform.position = parentPos + (unitVectorFromHorizontalAngleDeg(rotAngleDeg) * KnightDistance) + KnightDisOffset;
    }

    private Vector3 unitVectorFromHorizontalAngleDeg(float angle)
    {
        var XnegativeMultiplier = angle > 90 && angle < 270 ? -1 : 1;

        var y = Mathf.Sin(angle * ( (2*Mathf.PI) / 360));
        var x = Mathf.Sqrt(1 - (y * y)) * XnegativeMultiplier;
        return new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Spirit")
        {
            var bounceBackController = collision.gameObject.GetComponentInChildren<SpriteBounceBackController>();
            bounceBackController.Return();
            ParentKnight.ShieldHit();
        }
    }

    public void Hit()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.8f);
    }

    public void HitOver()
    {
        spriteRenderer.color = startcolor;
    }

    public void PauseShield()
    {
        boxCollider.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 0.2f);
    }

    public void UnPauseSheild()
    {
        boxCollider.enabled = true;
        spriteRenderer.color = startcolor;
    }
}
