using System.Linq;
using UnityEngine;

public class BlobAI : BasicEnemyAI
{
    public Rigidbody2D rb;

    [SerializeField]
    private float moveFrequency;
    [SerializeField]
    private float moveDistance;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject deathSpawnPrefab;
    [SerializeField]
    private int deathSpawnAmount;

    [SerializeField]
    private float scatterVecoity = 2f;


    private float moveTime = 5.0f / 6.0f; // frames / (frames / sec)
    private Coroutine currentCoroutine = null;

    // Start is called before the first frame update
    override protected void Start()
    {
        invicible = true;
        Invoke("setDamagable", 0.1f);
        InvokeRepeating("Move", 1.0f, moveFrequency);
        base.Start();
    }

    void setDamagable()
    {
        invicible = false;
    }

    void Move()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        GameObject closestPlayer = AIHelpers.GetClosestPlayer(transform.position);
        if(closestPlayer != null)
        {
            Vector3 destination = Vector3.MoveTowards(rb.position, closestPlayer.transform.position, moveDistance);
            float speed = Vector2.Distance(rb.position, destination) / moveTime;

            animator.SetFloat("lastHorizontal", destination.x - rb.position.x);
            animator.SetBool("isMoving", true);
            currentCoroutine = StartCoroutine(AIHelpers.MoveTo(
                rb,
                destination,
                speed, 
                () => animator.SetBool("isMoving", false)
            ));
        }
    }

    private void GameObjectToEmpty()
    {
        foreach (Component c in GetComponents(typeof(Component)))
        {
            if (c != transform)
            {
                Destroy(c);
            }
        }
    }

    private bool ShouldSpawnSmaller()
    {
        return (deathSpawnPrefab != null) && (deathSpawnAmount > 0);
    }

    private bool IsLastChild(GameObject go)
    {
        return go.transform.parent != null && go.transform.parent.name.Contains("blob") && (NumSurvivingChildren(go.transform.parent) == 1);
    }

    static private int NumSurvivingChildren(Transform parent)
    {
        int c = 0;
        foreach(Transform child in parent)
        {
            if (child.gameObject != null && child.gameObject.tag != "Destroyed")
                c += 1;
        }
        return c;
    }

    private GameObject GetEmptyAncestor()
    {
        GameObject currentEmptyAncestor = null;
        var currentGameObject = gameObject;
        while (IsLastChild(currentGameObject))
        {
            currentEmptyAncestor = currentGameObject.transform.parent.gameObject;
            currentGameObject = currentGameObject.transform.parent.gameObject;
        }
        return currentEmptyAncestor;
    }
    override public void Kill()
    {
        if (ShouldSpawnSmaller())
        {
            SpawnDeathParticles();
            GameObjectToEmpty();
            foreach (int _ in Enumerable.Range(0, deathSpawnAmount))
            {
                GameObject instance = Instantiate(deathSpawnPrefab);
                instance.transform.parent = transform;
                instance.transform.position = transform.position;
                instance.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * scatterVecoity;
            }
        } else
        {
            GameObject emptyAncestor = GetEmptyAncestor();
            if (emptyAncestor != null)
            {
                emptyAncestor.tag = "Destroyed";
                Destroy(emptyAncestor);
            }
            base.Kill();
        }
    }
}
