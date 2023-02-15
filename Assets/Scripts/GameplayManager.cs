using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStateManager;
using static ScoreManager;
using static LevelManager;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private GameObject[] _spawnPositions;
    private string[] _controlSchemes = { "Keyboard_1", "Keyboard_2", "Controller_1", "Controller_2" };
    private List<InputDevice> _inputDevices = new List<InputDevice>();
    private int _controllerNum = 0;

    [SerializeField] private List<PlayerInput> _playerInputs = new List<PlayerInput>();

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

    private void Start()
    {
        _inputDevices.Add(Keyboard.current);
        _inputDevices.Add(Keyboard.current);
    }

    public void ControllerCheck()
    {
        if (_controllerNum < Gamepad.all.Count)
        {
            int numConnected = Gamepad.all.Count - _controllerNum;
            for (int i = 0; i < numConnected; i++)
            {
                _inputDevices.Add(Gamepad.all[i]);
            }
            _controllerNum += numConnected;
        }
        else if (_controllerNum > Gamepad.all.Count)
        {
            int numDisconnected = _controllerNum - Gamepad.all.Count;
            for (int i = 0; i < numDisconnected; i++)
            {
                _inputDevices.Remove(Gamepad.all[i]);
            }
            _controllerNum -= numDisconnected;
        }
    }

    public void OnSceneLoaded()
    {
        _spawnPositions = GameObject.FindGameObjectsWithTag("SpawnPosition");
        ControllerCheck();
        SpawnPlayers();

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

        I_LevelManager.StartPause();
    }

    private void SpawnPlayers()
    {
        _playerInputs.Clear();

        for (int i = 0; i < _playerCount; i++)
        {
            PlayerInput newPlayer = PlayerInput.Instantiate(_playerPrefabs[i], controlScheme: _controlSchemes[i], pairWithDevice: _inputDevices[i]);
            newPlayer.transform.position = _spawnPositions[i].transform.position;
            newPlayer.GetComponent<PlayerFunctions>().PlayerNum = i + 1;
            _playerInputs.Add(newPlayer);
            newPlayer.enabled = false;
        }
    }

    public void EnablePlayers()
    {
        for (int i = 0; i < _playerInputs.Count; i++)
        {
            _playerInputs[i].enabled = true;
            _playerInputs[i].SwitchCurrentControlScheme(_controlSchemes[i], _inputDevices[i]);
        }
    }

    public void DisablePlayers()
    {
        for (int i = 0; i < _playerInputs.Count; i++)
        {
            _playerInputs[i].enabled = false;
        }
    }
}
