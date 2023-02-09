using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Gameplay
    }

    [SerializeField] private GameState _gameState;
    public GameState CurrentGameState { get => _gameState; set => _gameState = value; }

    public static GameStateManager Instance { get; set; }

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
        //_gameState = GameState.MainMenu;
    }

    public void EnterGame()
    {
        _gameState = GameState.Gameplay;
        FadeCanvas.Instance.FadeOut();
        SceneLoader.Instance.Load(SceneLoader.Scene.Gameplay);
    }
}
