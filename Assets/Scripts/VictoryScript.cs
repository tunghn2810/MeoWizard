using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScript : MonoBehaviour
{
    [SerializeField] private Image _victoryLine;
    [SerializeField] private Sprite[] _victoryLineAssets;

    [SerializeField] private GameObject[] _victoryPoseAssets;

    public void VictorySetup(int playerNum)
    {
        _victoryLine.sprite = _victoryLineAssets[playerNum - 1];
        Instantiate(_victoryPoseAssets[playerNum - 1]);
    }
}
