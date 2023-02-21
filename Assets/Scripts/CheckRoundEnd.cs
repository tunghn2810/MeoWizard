using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoundEnd : MonoBehaviour
{
    [SerializeField] private int _playerNum;

    public void EndDying()
    {
        LevelManager.I_LevelManager.AlreadyDead(_playerNum);
    }
}
