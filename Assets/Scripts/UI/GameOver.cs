using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{

    [SerializeField]
    private GameObject defaultButton;
    [SerializeField]
    private UnityEngine.EventSystems.EventSystem ES;

    public void OnEnable()
    {
        Time.timeScale = 0;
        ES.gameObject.SetActive(true);
    }

    public void Restart()
    {
        DungeonStateInfo.levelIndex = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        DungeonStateInfo.levelIndex = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }
}
