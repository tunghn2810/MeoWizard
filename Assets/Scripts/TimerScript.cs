using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static LevelManager;
using static Alphabet;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private Image _minute;
    [SerializeField] private Image _secondx10;
    [SerializeField] private Image _second;

    private void Update()
    {
        DisplayTime(I_LevelManager.RoundTimer);
    }

    public void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        int secondsx10 = Mathf.FloorToInt(seconds/10);
        int second = Mathf.FloorToInt(seconds - (secondsx10*10));

        _minute.sprite = I_Alphabet.BigNumbers[minutes];
        _secondx10.sprite = I_Alphabet.BigNumbers[secondsx10];
        _second.sprite = I_Alphabet.BigNumbers[second];
    }
}
