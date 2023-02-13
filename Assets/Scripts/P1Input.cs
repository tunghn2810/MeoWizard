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
        //_playerInputActions = new PlayerInputActions();
        //EnableInput();
    }

    private void OnEnable()
    {
        //_playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        //_playerInputActions.Player.Disable();
    }

    public void EnableInput()
    {
        _playerInputActions.Player.MoveUp.performed += DirectionalInput;
        _playerInputActions.Player.MoveDown.performed += DirectionalInput;
        _playerInputActions.Player.MoveLeft.performed += DirectionalInput;
        _playerInputActions.Player.MoveRight.performed += DirectionalInput;
        
        _playerInputActions.Player.MoveUp.canceled += DirectionalInput;
        _playerInputActions.Player.MoveDown.canceled += DirectionalInput;
        _playerInputActions.Player.MoveLeft.canceled += DirectionalInput;
        _playerInputActions.Player.MoveRight.canceled += DirectionalInput;
        
        _playerInputActions.Player.A.performed += AInput;
    }

    public void DirectionalInput(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
            I_InputProcessor.DirectionalButton(context, _playerFunctions);
    }

    public void AInput(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
            I_InputProcessor.AButton(context, _playerFunctions);
    }
}
