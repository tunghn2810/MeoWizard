using UnityEngine;
using UnityEngine.InputSystem;

public class P1Input : MonoBehaviour
{
    private PlayerFunctions _playerFunctions;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        //_playerFunctions = GameObject.FindGameObjectWithTag("P1").GetComponent<PlayerFunctions>();
        _playerFunctions = GetComponent<PlayerFunctions>();
        _playerInputActions = new PlayerInputActions();
        EnableInput();
    }

    private void OnDisable()
    {
        _playerInputActions.Player_1.Disable();
    }

    public void EnableInput()
    {
        _playerInputActions.Player_1.Enable();

        _playerInputActions.Player_1.MoveUp.performed += DirectionalInput;
        _playerInputActions.Player_1.MoveDown.performed += DirectionalInput;
        _playerInputActions.Player_1.MoveLeft.performed += DirectionalInput;
        _playerInputActions.Player_1.MoveRight.performed += DirectionalInput;

        _playerInputActions.Player_1.MoveUp.canceled += DirectionalInput;
        _playerInputActions.Player_1.MoveDown.canceled += DirectionalInput;
        _playerInputActions.Player_1.MoveLeft.canceled += DirectionalInput;
        _playerInputActions.Player_1.MoveRight.canceled += DirectionalInput;

        _playerInputActions.Player_1.A.performed += AInput;
    }

    private void DirectionalInput(InputAction.CallbackContext context)
    {
        InputProcessor.Instance.DirectionalButton(context, _playerFunctions);
    }

    private void AInput(InputAction.CallbackContext context)
    {
        InputProcessor.Instance.AButton(context, _playerFunctions);
    }
}
