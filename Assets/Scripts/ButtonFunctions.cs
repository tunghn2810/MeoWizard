using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void PlayGame()
    {
        GameStateManager.Instance.EnterGame();
        SceneManager.LoadScene("Demo_1");
    }
}
