using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static GameplayManager;
using static Alphabet;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private List<int> _scoreboard = new List<int>();
    private GameObject _scoreboardPlayers;
    [SerializeField] private Image[] _scoreSprites = new Image[4];

    private int _endScore = 1;
    public int EndScore { get => _endScore; set => _endScore = value; }

    private bool _isGameEnd = false;
    public bool IsGameEnd { get => _isGameEnd; }

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

    public void AdjustScoreSprites()
    {
        for (int i = 0; i < I_GameplayManager.PlayerCount; i++)
        {
            _scoreSprites[i].sprite = I_Alphabet.SmallNumbers[_scoreboard[i]];
        }
    }

    public void AddScore(int playerNum)
    {
        _scoreboard[playerNum - 1] += 1;
        _scoreSprites[playerNum - 1].sprite = I_Alphabet.SmallNumbers[_scoreboard[playerNum - 1]];

        if (_scoreboard[playerNum - 1] == _endScore)
        {
            _isGameEnd = true;
        }
    }

    public void JoinGame(int playerNum)
    {
        _scoreboard.Insert(playerNum - 1, 0);
    }

    public void ResetScores()
    {
        _scoreboard.Clear();
        _isGameEnd = false;
    }
}
