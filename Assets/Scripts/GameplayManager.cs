using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStateManager;
using static ScoreManager;
using static LevelManager;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _players;
    [SerializeField] private int _playerCount;
    public int PlayerCount { get => _playerCount; set => _playerCount = value; }
    
    public static GameplayManager I_GameplayManager { get; set; }
    private void Awake()
    {
        if (I_GameplayManager == null)
        {
            I_GameplayManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = _playerCount; i < _players.Length; i++)
        {
            _players[i].SetActive(false);
        }

        I_ScoreManager.OnSceneLoaded();

        if (I_GameStateManager.EnterGameplay)
        {
            for (int i = 0; i < _playerCount; i++)
            {
                I_ScoreManager.JoinGame(i + 1);
            }
            I_GameStateManager.EnterGameplay = false;
        }

        for (int i = 0; i < _playerCount; i++)
        {
            I_LevelManager.AddPlayer(i + 1);
        }

        I_ScoreManager.AdjustScoreSprites();
    }
}
