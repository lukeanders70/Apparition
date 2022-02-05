using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthContainerController : MonoBehaviour
{
    public PlayerHealth ph;
    public GameObject healthImage;

    private float distanceBetweenHealthImages = 12.0f;
    private Stack<GameObject> healthImages = new Stack<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        int healthAmount = PlayerStateInfo.health;
        for(int i = 0; i < healthAmount; i++)
        {
            AddHealth();
        }

    }

    private void AddHealth()
    {
        int i = healthImages.Count;
        GameObject image = Instantiate(healthImage, transform);
        image.transform.localPosition = new Vector3(i * distanceBetweenHealthImages, 0, 0);
        healthImages.Push(image);
    }

    private void RemoveHealth()
    {
        Destroy(healthImages.Pop());
    }
    
    public void UpdateHealth(int newHealth)
    {
        while(healthImages.Count != newHealth)
        {
            if(healthImages.Count > newHealth)
            {
                RemoveHealth();
            } else
            {
                AddHealth();
            }
        }
    }
}
