using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static SceneLoader;
using static FadeCanvas;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Loading,
        MainMenu,
        Gameplay,
        Victory
    }

    [SerializeField] private GameState _gameState;
    public GameState CurrentGameState { get => _gameState; set => _gameState = value; }

    [SerializeField] private GameState _nextGameState;
    public GameState NextGameState { get => _nextGameState; }

    [SerializeField] private bool _enterGameplay = false;
    public bool EnterGameplay { get => _enterGameplay; set => _enterGameplay = value; }

    public static GameStateManager I_GameStateManager { get; set; }

    private void Awake()
    {
        if (I_GameStateManager == null)
        {
            I_GameStateManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetNextState(GameState nextState)
    {
        _nextGameState = nextState;
    }

    public void EnterLoading()
    {
        I_FadeCanvas.FadeOut();
        I_SceneLoader.Load("Loading");
    }

    public void EnterGame()
    {
        I_FadeCanvas.FadeOut();
        _gameState = GameState.Gameplay;
        I_SceneLoader.Load("Gameplay");
    }

    public void EnterVictory()
    {
        I_FadeCanvas.FadeOut();
        _gameState = GameState.Victory;
        I_SceneLoader.Load("Victory");
    }

    public void EnterMenu()
    {
        I_FadeCanvas.FadeOut();
        _gameState = GameState.MainMenu;
        I_SceneLoader.Load("MainMenu");
    }
}
