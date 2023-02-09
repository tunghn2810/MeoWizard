using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        MainMenu,
        Gameplay
    }

    private Action onLoaderCallback;

    public static SceneLoader Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
            StartCoroutine(LoadAsync(scene));
        };
    }

    private IEnumerator LoadAsync(Scene scene)
    {
        yield return null; //Do nothing for 1 frame
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        while (!FadeCanvas.Instance.IsFadeOutFinished)
        {
            yield return null;
        }

        FadeCanvas.Instance.FadeIn();
    }

    public void LoaderCallback()
    {
        if (onLoaderCallback != null)
            onLoaderCallback();
        onLoaderCallback = null;
    }
}
