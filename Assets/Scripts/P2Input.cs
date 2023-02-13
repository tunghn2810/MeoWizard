using UnityEngine;
using UnityEngine.InputSystem;

using static InputProcessor;

public class P2Input : MonoBehaviour
{
    //private PlayerFunctions _playerFunctions;
    //private PlayerInputActions _playerInputActions;
    //
    //private void Awake()
    //{
    //    _playerFunctions = GetComponent<PlayerFunctions>();
    //    _playerInputActions = new PlayerInputActions();
    //    EnableInput();
    //}
    //
    //private void OnEnable()
    //{
    //    _playerInputActions.Player_2.Enable();
    //}
    //
    //private void OnDisable()
    //{
    //    _playerInputActions.Player_2.Disable();
    //}
    //
    //public void EnableInput()
    //{
    //    _playerInputActions.Player_2.MoveUp.performed += MoveInput;
    //    _playerInputActions.Player_2.MoveDown.performed += MoveInput;
    //    _playerInputActions.Player_2.MoveLeft.performed += MoveInput;
    //    _playerInputActions.Player_2.MoveRight.performed += MoveInput;
    //
    //    _playerInputActions.Player_2.MoveUp.canceled += MoveInput;
    //    _playerInputActions.Player_2.MoveDown.canceled += MoveInput;
    //    _playerInputActions.Player_2.MoveLeft.canceled += MoveInput;
    //    _playerInputActions.Player_2.MoveRight.canceled += MoveInput;
    //
    //    _playerInputActions.Player_2.A.performed += PlantBomb;
    //}
    //
    //private void MoveInput(InputAction.CallbackContext context)
    //{
    //    I_InputProcessor.DirectionalButton(context, _playerFunctions);
    //}
    //
    //private void PlantBomb(InputAction.CallbackContext context)
    //{
    //    I_InputProcessor.AButton(context, _playerFunctions);
    //}
}
