using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProcessor : MonoBehaviour
{
    private MenuControlScript _menuControl;

    public static InputProcessor Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _menuControl = GameObject.FindGameObjectWithTag("MenuControl").GetComponent<MenuControlScript>();
    }

    public void DirectionalButton(InputAction.CallbackContext context, PlayerFunctions player)
    {
        int directionNum = -1;
        if (context.action.name == "MoveUp")
        {
            directionNum = 0;
        }
        if (context.action.name == "MoveDown")
        {
            directionNum = 1;
        }
        if (context.action.name == "MoveLeft")
        {
            directionNum = 2;
        }
        if (context.action.name == "MoveRight")
        {
            directionNum = 3;
        }

        if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.MainMenu)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (directionNum == 0)
                    _menuControl.MoveCursor(false);
                else if (directionNum == 1)
                    _menuControl.MoveCursor(true);
            }
        }
        else if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.Gameplay)
        {
            player.MoveInput(directionNum);
        }
    }

    public void AButton(InputAction.CallbackContext context, PlayerFunctions player)
    {
        if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.MainMenu)
        {
            if (context.phase == InputActionPhase.Performed)
                _menuControl.Submit();
        }
        else if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.Gameplay)
            player.PlantBomb();
    }
}
