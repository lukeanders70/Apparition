using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject spirit;

    public void OnSwap(InputAction.CallbackContext value)
    {
        bool attemptSwap = value.started;
    }

    public void OnMovePlayer1(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();

    }

    public void OnMovePlayer2(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
    }
}
