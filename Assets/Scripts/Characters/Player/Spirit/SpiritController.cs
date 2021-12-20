using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritController : MonoBehaviour
{
    [SerializeField]
    private int speed = 10;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private SpriteRenderer spriteRendererWhite;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speedMultiplier;

    public ParticleSystem hitPs;
    public ParticleSystem tailPs;

    private ParticleSystem activeTailPs;

    public bool isMoving = false;
    public GameObject lastParent;
    public GameObject futureParent;
    private Coroutine currentCoroutine;

    public void Start()
    {
        activeTailPs = Instantiate(tailPs);
        activeTailPs.transform.position = transform.position;
        activeTailPs.transform.parent = transform;
        activeTailPs.Stop();
    }

    public void Swap(GameObject newParent)
    {
        // set position to parents position and remove parent
        transform.position = transform.parent.transform.position;
        lastParent = transform.parent.gameObject;
        futureParent = newParent;
        transform.parent = transform.parent.transform.parent;

        makeOpaque();

        StartExclusiveMove(newParent);
    }

    public void StartExclusiveMove(GameObject newParent)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Move(newParent));
    }

    private IEnumerator Move(GameObject newParent)
    {
        isMoving = true;
        Vector3 targetPosition = newParent.transform.position;
        activeTailPs.Play();
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            targetPosition = newParent.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }
        transform.SetParent(newParent.transform);
        makeTransparent();
        transform.localPosition = Vector3.zero;
        activeTailPs.Stop();
        isMoving = false;
        newParent.GetComponent<SpiritHandler>().ReceiveSpirit();
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.GetComponent<Collider2D>().gameObject;
        if (isMoving && collidedObject.tag == "Enemy")
        {
            ParticleSystem hitParticals = Instantiate(hitPs);
            hitParticals.transform.position = transform.position;
            Health healthComponent = collidedObject.GetComponent<Health>();
            if(healthComponent != null)
            {
                healthComponent.Damage(damage);
            }
        } else if (isMoving && collidedObject.tag == "Wall" && collidedObject.layer == 7)
        {
            spriteRendererWhite.sortingOrder = 1;
        }
    }

    private void makeTransparent()
    {
        spriteRendererWhite.color = Color.clear;
    }

    private void makeOpaque()
    {
        spriteRendererWhite.color = Color.white;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.GetComponent<Collider2D>().gameObject;
        if (isMoving && collidedObject.tag == "Wall" && collidedObject.layer == 7)
        {
            spriteRendererWhite.sortingOrder = 0;
        }
    }
}
