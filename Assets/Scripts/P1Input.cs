using UnityEngine;
using UnityEngine.InputSystem;

public class P1Input : MonoBehaviour
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
        _playerInputActions.Player_1.Enable();

        _playerInputActions.Player_1.MoveUp.performed += MoveInput;
        _playerInputActions.Player_1.MoveDown.performed += MoveInput;
        _playerInputActions.Player_1.MoveLeft.performed += MoveInput;
        _playerInputActions.Player_1.MoveRight.performed += MoveInput;

        _playerInputActions.Player_1.MoveUp.canceled += MoveInput;
        _playerInputActions.Player_1.MoveDown.canceled += MoveInput;
        _playerInputActions.Player_1.MoveLeft.canceled += MoveInput;
        _playerInputActions.Player_1.MoveRight.canceled += MoveInput;

        _playerInputActions.Player_1.A.performed += PlantBomb;
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
