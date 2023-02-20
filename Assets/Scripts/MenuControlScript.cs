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
    [SerializeField] private Image _playerNumbers;
    [SerializeField] private Sprite[] _playerNumbersSprites;
    private int _spriteIndex = 0;

    //End score
    [SerializeField] private Image _rounds;
    [SerializeField] private Sprite[] _roundsSprites;
    private int _endScore = 1;

    //Cursor control
    private RectTransform _cursor;
    private int _cursorIndex = 1;
    private Vector2 _cursorOffset = new Vector2(-24, 0);

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
        _cursor = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Start()
    {
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
            _playerNumbers.sprite = _playerNumbersSprites[_spriteIndex];
        }
        else
        {
            _spriteIndex = 0;
            _playerNumbers.sprite = _playerNumbersSprites[_spriteIndex];
        }

        I_GameplayManager.PlayerCount = _spriteIndex + 2;
    }

    public void AdjustEndScore()
    {
        if (_endScore < _roundsSprites.Length)
        {
            _endScore += 1;
            _rounds.sprite = _roundsSprites[_endScore - 1];
        }
        else
        {
            _endScore = 1;
            _rounds.sprite = _roundsSprites[_endScore - 1];
        }

        I_ScoreManager.EndScore = _endScore;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        if (!Application.isEditor)
        {
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Quit();
        }
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
