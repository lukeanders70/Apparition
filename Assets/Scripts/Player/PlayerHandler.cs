using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public GameObject getOtherPlayer(GameObject player)
    {
        return player == player1 ? player2 : player1;
    }

    public void Kill()
    {
        player1.GetComponent<Health>().Kill();
        player2.GetComponent<Health>().Kill();
    }
}
