using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P2Input : MonoBehaviour
{
    private PlayerFunctions _playerFunctions;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerFunctions = GetComponent<PlayerFunctions>();
        _playerInputActions = new PlayerInputActions();
        EnableInput();
    }

    public void EnableInput()
    {
        _playerInputActions.Player_2.Enable();

        _playerInputActions.Player_2.MoveUp.performed += MoveInput;
        _playerInputActions.Player_2.MoveDown.performed += MoveInput;
        _playerInputActions.Player_2.MoveLeft.performed += MoveInput;
        _playerInputActions.Player_2.MoveRight.performed += MoveInput;

        _playerInputActions.Player_2.MoveUp.canceled += MoveInput;
        _playerInputActions.Player_2.MoveDown.canceled += MoveInput;
        _playerInputActions.Player_2.MoveLeft.canceled += MoveInput;
        _playerInputActions.Player_2.MoveRight.canceled += MoveInput;

        _playerInputActions.Player_2.A.performed += PlantBomb;
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        _playerFunctions.MoveInput(context);
    }

    private void PlantBomb(InputAction.CallbackContext context)
    {
        _playerFunctions.PlantBomb();
    }
}