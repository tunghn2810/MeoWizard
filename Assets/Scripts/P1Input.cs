using UnityEngine;
using UnityEngine.InputSystem;

using static InputProcessor;

public class P1Input : MonoBehaviour
{
    private PlayerFunctions _playerFunctions;

    private void Awake()
    {
        _playerFunctions = GetComponent<PlayerFunctions>();
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
