using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using static GameStateManager;

public class InputProcessor : MonoBehaviour
{
    private MenuControlScript _menuControl;

    public static InputProcessor I_InputProcessor { get; set; }
    private void Awake()
    {
        if (I_InputProcessor == null)
        {
            I_InputProcessor = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded()
    {
        _menuControl = GameObject.FindGameObjectWithTag("MenuControl")?.GetComponent<MenuControlScript>();
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

        if (I_GameStateManager.CurrentGameState == GameState.MainMenu || I_GameStateManager.CurrentGameState == GameState.Victory)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (directionNum == 0)
                    _menuControl.MoveCursor(false);
                else if (directionNum == 1)
                    _menuControl.MoveCursor(true);
            }
        }
        else if (I_GameStateManager.CurrentGameState == GameState.Gameplay)
        {
            player.MoveInput(directionNum);
        }
    }

    public void AButton(InputAction.CallbackContext context, PlayerFunctions player)
    {
        if (I_GameStateManager.CurrentGameState == GameState.MainMenu || I_GameStateManager.CurrentGameState == GameState.Victory)
        {
            if (context.phase == InputActionPhase.Performed)
                _menuControl.Submit();
        }
        else if (I_GameStateManager.CurrentGameState == GameState.Gameplay)
            player.PlantBomb();
    }
}
