using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSplashController : MonoBehaviour
{
    public GameObject splashImageContainer;
    public List<GameObject> spashImages;
    public Text levelNameText;

    private float levelImageHeight = 3.0f;

    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private float scrollTime;
    [SerializeField]
    private float levelStartTime;

    // Start is called before the first frame update
    void Start()
    {
        if(DungeonStateInfo.levelIndex < StaticDungeon.LevelIndex.levels.Length)
        {
            // More Levels to go, load up next one
            var uninitOffset = splashImageContainer.transform.position.y;
            var startingOffset = uninitOffset + Mathf.Max(0, DungeonStateInfo.levelIndex - 1) * levelImageHeight;
            var endingOffset = uninitOffset + DungeonStateInfo.levelIndex * levelImageHeight;
            splashImageContainer.transform.position = splashImageContainer.transform.position + new Vector3(0f, startingOffset, 0f);
            for (int i = 0; i < DungeonStateInfo.levelIndex; i++)
            {
                spashImages[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
            levelNameText.text = StaticDungeon.LevelIndex.levels[DungeonStateInfo.levelIndex].Name;
            StartCoroutine(FadeIn(spashImages[DungeonStateInfo.levelIndex].GetComponent<SpriteRenderer>(), fadeTime, () => {
                StartCoroutine(Scroll(scrollTime, endingOffset, () => {
                    levelNameText.color = Color.white;
                    Invoke("StartLevel", levelStartTime);
                }));
            }));
        } else
        {
            // Game is complete
            levelNameText.text = "";            
            StartCoroutine(FadeIn(spashImages[DungeonStateInfo.levelIndex].GetComponent<SpriteRenderer>(), fadeTime, () => {
                StartCoroutine(Scroll(scrollTime, 0, () => {
                    levelNameText.color = Color.white;
                    Invoke("Exit", 10);
                }));
            }));
        }

    }

    private IEnumerator FadeIn(SpriteRenderer renderer, float time, Action callback)
    {
        var elapsedTime = 0f;
        var fullTransparency = 1.0;
        while(elapsedTime < time)
        {
            float newTransparency = (float) ((elapsedTime / time) * fullTransparency);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, newTransparency);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        callback();
        renderer.color = Color.white;
        yield break;
    }

    private IEnumerator Scroll(float time, float endingOffset, Action callback)
    {
        var elapsedTime = 0f;
        var startingOffset = splashImageContainer.transform.position.y;
        while (elapsedTime < time)
        {
            var newOffset = ((1 - (elapsedTime / time)) * startingOffset) + ((elapsedTime / time) * endingOffset);
            splashImageContainer.transform.position = new Vector3(splashImageContainer.transform.position.x, newOffset, splashImageContainer.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        splashImageContainer.transform.position = new Vector3(splashImageContainer.transform.position.x, endingOffset, splashImageContainer.transform.position.z);
        callback();
        yield break;
    }
    
    private void StartLevel()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }
}
