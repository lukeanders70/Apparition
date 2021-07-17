using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{

    [SerializeField]
    GameObject defaultButton;
    [SerializeField]
    UnityEngine.EventSystems.EventSystem ES;

    public void OnEnable()
    {
        ES.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
