using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static GameStateManager;
using static GameplayManager;
using static ScoreManager;

public class MenuControlScript : MonoBehaviour
{
    //Buttons
    private Button[] _buttons;

    //Number of players
    [SerializeField] private Sprite[] _playerNumbersSprites;
    private int _spriteIndex = 0;

    //End score
    [SerializeField] private Sprite[] _endScoreSprites;
    private int _endScore = 1;

    //Cursor control
    private RectTransform _cursor;
    private int _cursorIndex = 1;
    private Vector2 _cursorOffset = new Vector2(-64, 0);

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
        _cursor = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Start()
    {
        I_GameplayManager.PlayerCount = _spriteIndex + 2;
        _cursor.anchoredPosition = _buttons[_cursorIndex - 1].GetComponent<RectTransform>().anchoredPosition + _cursorOffset;
    }

    public void MoveCursor(bool isMoveDown)
    {
        if (isMoveDown)
        {
            if (_cursorIndex < transform.childCount - 1)
            {
                _cursorIndex += 1;
                _cursor.anchoredPosition = _buttons[_cursorIndex - 1].GetComponent<RectTransform>().anchoredPosition + _cursorOffset;
            }
        }
        else
        {
            if (_cursorIndex > 1)
            {
                _cursorIndex -= 1;
                _cursor.anchoredPosition = _buttons[_cursorIndex - 1].GetComponent<RectTransform>().anchoredPosition + _cursorOffset;
            }
        }
    }

    public void Submit()
    {
        _buttons[_cursorIndex - 1].Submit();
    }

    public static void PlayGame()
    {
        I_GameStateManager.EnterGameplay = true;
        I_GameStateManager.SetNextState(GameState.Gameplay);
        I_GameStateManager.EnterLoading();
    }

    public void AdjustPlayerCount()
    {
        if (_spriteIndex < _playerNumbersSprites.Length - 1)
        {
            _spriteIndex += 1;
            _buttons[1].GetComponent<Image>().sprite = _playerNumbersSprites[_spriteIndex];
        }
        else
        {
            _spriteIndex = 0;
            _buttons[1].GetComponent<Image>().sprite = _playerNumbersSprites[_spriteIndex];
        }

        I_GameplayManager.PlayerCount = _spriteIndex + 2;
    }

    public void AdjustEndScore()
    {
        if (_endScore < 5)
        {
            _endScore += 1;
            _buttons[2].GetComponent<Image>().sprite = _endScoreSprites[_spriteIndex];
        }
        else
        {
            _endScore = 0;
            _buttons[2].GetComponent<Image>().sprite = _endScoreSprites[_spriteIndex];
        }

        I_ScoreManager.EndScore = _endScore;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public static void Replay()
    {
        I_GameStateManager.EnterGameplay = true;
        I_ScoreManager.ResetScores();
        PlayGame();
    }

    public static void BackToMenu()
    {
        I_GameStateManager.EnterGameplay = true;
        I_ScoreManager.ResetScores();
        I_GameStateManager.SetNextState(GameState.MainMenu);
        I_GameStateManager.EnterLoading();
    }
}
