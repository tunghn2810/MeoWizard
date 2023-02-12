using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStateManager;
using static ScoreManager;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<int> _alivePlayers = new List<int>();

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

    private IEnumerator EndGamePause()
    {
        yield return new WaitForSeconds(5f);
        ResetPlayers();

        I_GameStateManager.SetNextState(GameState.Gameplay);
        I_GameStateManager.EnterLoading();
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
}
