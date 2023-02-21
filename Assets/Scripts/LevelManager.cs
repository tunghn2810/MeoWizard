using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStateManager;
using static GameplayManager;
using static ScoreManager;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<int> _alivePlayers = new List<int>();

    private float _roundTimer = 180f;
    public float RoundTimer { get => _roundTimer; set => _roundTimer = value; }
    private const float MAX_ROUND_TIMER = 180f;
    private bool _isPlaying = false;

    [SerializeField] private List<int> _dying = new List<int>();
    private int _winner = 0;
    private VictoryScript _victoryScript;
    public event Action OnWin;

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
            if (_roundTimer <= 0)
            {
                _roundTimer = 0;
                _isPlaying = false;
            }

            _roundTimer -= Time.deltaTime;
        }

        RoundEndCheck();
    }

    public void RoundEndCheck()
    {
        if (!_isPlaying)
            return;

        if (_dying.Count > 0)
            return;

        if (_alivePlayers.Count > 1)
            return;
        else if (_alivePlayers.Count == 1)
        {
            I_ScoreManager.AddScore(_alivePlayers[0]);
            _winner = _alivePlayers[0];
        }
        else if (_alivePlayers.Count < 1)
        {
            //DRAW - No player gets score.
        }

        StartCoroutine(EndGamePause());
    }

    public void StartPause()
    {
        StartCoroutine(StartGamePause());
    }

    public void OnVictory()
    {
        _victoryScript = GameObject.FindGameObjectWithTag("VictoryScript").GetComponent<VictoryScript>();
        _victoryScript.VictorySetup(_winner);
    }

    private IEnumerator StartGamePause()
    {
        yield return new WaitForSeconds(1f);

        I_GameplayManager.EnablePlayers();

        _isPlaying = true;
    }

    private IEnumerator EndGamePause()
    {
        _isPlaying = false;
        I_GameplayManager.DisablePlayers();
        OnWin?.Invoke();

        yield return new WaitForSeconds(3f);

        ResetPlayers();
        ResetTimer();

        if (I_ScoreManager.IsGameEnd)
        {
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
        Debug.Log(playerNum);
        _alivePlayers.Remove(playerNum);
        _dying.Add(playerNum);
    }

    public void AlreadyDead(int playerNum)
    {
        _dying.Remove(playerNum);
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
