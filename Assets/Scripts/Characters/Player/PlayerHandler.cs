using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    [SerializeField]
    private Vector2 player1StartPosition;
    [SerializeField]
    private Vector2 player2StartPosition;
    [SerializeField]
    private GameObject PauseMenu;

    public void Awake()
    {
        resetPosition();
    }

    private void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            if (PauseMenu.activeSelf) {
                PauseMenu.GetComponent<PauseController>().Resume();
            } else {
                PauseMenu.SetActive(true);
            }
        }
    }

    public GameObject getOtherPlayer(GameObject player)
    {
        return player == player1 ? player2 : player1;
    }

    public void SetColor(Color c)
    {
        player1.GetComponent<SpriteRenderer>().color = c;
        player2.GetComponent<SpriteRenderer>().color = c;
    }

    public void Kill()
    {
        player1.GetComponent<Health>().Kill();
        player2.GetComponent<Health>().Kill();
    }

    public void resetPosition()
    {
        player1.GetComponent<PlayerMovement>().teleportTranslate(player1StartPosition);
        player2.GetComponent<PlayerMovement>().teleportTranslate(player2StartPosition);

    }

    public void PauseMovement()
    {
        player1.GetComponent<PlayerMovement>().StopMove();
        player2.GetComponent<PlayerMovement>().StopMove();
    }
    public void UnPauseMovement()
    {
        player1.GetComponent<PlayerMovement>().StartMove();
        player2.GetComponent<PlayerMovement>().StartMove();
    }

    public GameObject GetActivePlayer()
    {
        if (player1.GetComponent<SpiritHandler>().spirit.transform.parent == player1.transform) {
            return player1;
        } else if (player2.GetComponent<SpiritHandler>().spirit.transform.parent == player2.transform)
        {
            return player2;
        }
        return null;
    }

    public GameObject GetInActivePlayer()
    {
        if (player1.GetComponent<SpiritHandler>().spirit.transform.parent != player1.transform)
        {
            return player1;
        }
        else
        {
            return player2;
        }
    }
}
