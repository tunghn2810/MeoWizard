using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static GameStateManager;
using static GameplayManager;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private List<int> _scoreboard = new List<int>();
    private GameObject _scoreboardPlayers;
    [SerializeField] private Image[] _scoreSprites = new Image[4];

    private int _endScore = 3;
    public int EndScore { get => _endScore; set => _endScore = value; }

    public static ScoreManager I_ScoreManager { get; set; }
    private void Awake()
    {
        if (I_ScoreManager == null)
        {
            I_ScoreManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded()
    {
        _scoreboardPlayers = GameObject.FindGameObjectWithTag("ScoreboardPlayers");
        for (int i = 0; i < _scoreboardPlayers.transform.childCount; i++)
        {
            _scoreSprites[i] = _scoreboardPlayers.transform.GetChild(i).GetComponent<Image>();
        }
        for (int i = I_GameplayManager.PlayerCount; i < _scoreboardPlayers.transform.childCount; i++)
        {
            _scoreboardPlayers.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (I_GameStateManager.CurrentGameState != GameState.Gameplay)
            return;

        if (_scoreboard.Contains(_endScore))
        {
            Debug.Log("Player " + (_scoreboard.IndexOf(_endScore) + 1).ToString() + " wins!");
            //ResetScores();
        }
    }

    public void AdjustScoreSprites()
    {
        for (int i = 0; i < I_GameplayManager.PlayerCount; i++)
        {
            _scoreSprites[i].sprite = Alphabet.Instance.SmallNumbers[_scoreboard[i]];
        }
    }

    public void AddScore(int playerNum)
    {
        _scoreboard[playerNum - 1] += 1;
        _scoreSprites[playerNum - 1].sprite = Alphabet.Instance.SmallNumbers[_scoreboard[playerNum - 1]];
    }

    public void JoinGame(int playerNum)
    {
        _scoreboard.Insert(playerNum - 1, 0);
    }

    public void ResetScores()
    {
        _scoreboard.Clear();
    }
}
