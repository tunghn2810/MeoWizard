using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static GameStateManager;
using static GameplayManager;
using static FadeCanvas;
using static InputProcessor;

public class SceneLoader : MonoBehaviour
{
    private Action onLoaderCallback;

    public static SceneLoader I_SceneLoader { get; set; }
    private void Awake()
    {
        if (I_SceneLoader == null)
        {
            I_SceneLoader = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Loading")
        {

        }
        else if (scene.name == "Gameplay")
        {
            I_GameplayManager.OnSceneLoaded();
        }
        else if (scene.name == "MainMenu")
        {
            I_InputProcessor.OnSceneLoaded();
        }
    }

    private void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Load(string sceneName)
    {
        onLoaderCallback = () =>
        {
            StartCoroutine(LoadAsync(sceneName));
        };
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        yield return null; //Do nothing for 1 frame
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        while (!I_FadeCanvas.IsFadeOutFinished)
        {
            yield return null;
        }

        I_FadeCanvas.FadeIn();

        yield return null;

        if (I_GameStateManager.NextGameState == GameState.Gameplay)
        {
            I_GameStateManager.EnterGame();
            I_GameStateManager.SetNextState(GameState.Loading);
        }
    }

    public void LoaderCallback()
    {
        onLoaderCallback?.Invoke();
        onLoaderCallback = null;
    }
}
