using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLootController : MonoBehaviour
{
    public float despawnTime = 0.0f;
    const float despawnWarningTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        if(despawnTime > 0)
        {
            Invoke("despawn", despawnTime);
            if(despawnTime - despawnWarningTime > 0)
            {
                Invoke("greyOut", despawnTime - despawnWarningTime);
            }
        }
    }

    private void despawn()
    {
        Destroy(gameObject);
    }

    private void greyOut()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.a / 2.0f);
        }
    }
}
