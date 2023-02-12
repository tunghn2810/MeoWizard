using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static GameStateManager;

public static class ButtonFunctions
{
    public static void PlayGame()
    {
        I_GameStateManager.EnterGameplay = true;
        I_GameStateManager.SetNextState(GameState.Gameplay);
        I_GameStateManager.EnterLoading();
    }

    public static void Replay()
    {
        I_GameStateManager.EnterGameplay = true;
        PlayGame();
    }
}
