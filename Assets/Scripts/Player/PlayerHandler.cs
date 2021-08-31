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

    public void Awake()
    {
        resetPosition();
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
}
