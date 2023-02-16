using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStateManager;
using static GameplayManager;
using static ScoreManager;

public class LevelManager : MonoBehaviour
{
    private List<int> _alivePlayers = new List<int>();

    private float _roundTimer = 180f;
    public float RoundTimer { get => _roundTimer; set => _roundTimer = value; }
    private const float MAX_ROUND_TIMER = 180f;
    private bool _isPlaying = false;

    public static LevelManager I_LevelManager { get; set; }
    private void Awake()
    {
        if (I_LevelManager == null)
        {
            I_LevelManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (I_ScoreManager.IsGameEnd)
            return;

        if (_isPlaying)
        {
            RoundEndCheck();

            if (_roundTimer <= 0)
            {
                _roundTimer = 0;
                _isPlaying = false;
            }

            _roundTimer -= Time.deltaTime;
        }
    }

    public void RoundEndCheck()
    {
        if (_alivePlayers.Count > 1)
        {
            return;
        }

        else if (_alivePlayers.Count == 1)
        {
            I_ScoreManager.AddScore(_alivePlayers[0]);
        }
        else if (_alivePlayers.Count < 1)
        {
            //DRAW
        }

        StartCoroutine(EndGamePause());
    }

    public void StartPause()
    {
        StartCoroutine(StartGamePause());
    }

    private IEnumerator StartGamePause()
    {
        yield return new WaitForSeconds(1f);

        I_GameplayManager.EnablePlayers();

        _isPlaying = true;
    }

    private IEnumerator EndGamePause()
    {
        yield return new WaitForSeconds(5f);
        ResetPlayers();
        ResetTimer();

        if (I_ScoreManager.IsGameEnd)
        {
            _isPlaying = false;
            I_GameStateManager.SetNextState(GameState.Victory);
            I_GameStateManager.EnterLoading();
        }
        else
        {
            I_GameStateManager.SetNextState(GameState.Gameplay);
            I_GameStateManager.EnterLoading();
        }
    }

    public void AddPlayer(int playerNum)
    {
        _alivePlayers.Insert(playerNum - 1, playerNum);
    }

    public void RemovePlayer(int playerNum)
    {
        _alivePlayers.Remove(playerNum);
    }

    public void ResetPlayers()
    {
        _alivePlayers.Clear();
    }

    public void ResetTimer()
    {
        _roundTimer = MAX_ROUND_TIMER;
    }
}
