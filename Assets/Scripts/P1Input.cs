using UnityEngine;
using UnityEngine.InputSystem;

using static InputProcessor;

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

    private void OnEnable()
    {
        _playerInputActions.Player_1.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player_1.Disable();
    }

    public void EnableInput()
    {
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

    public void DirectionalInput(InputAction.CallbackContext context)
    {
        I_InputProcessor.DirectionalButton(context, _playerFunctions);
    }

    public void AInput(InputAction.CallbackContext context)
    {
        I_InputProcessor.AButton(context, _playerFunctions);
    }
}
