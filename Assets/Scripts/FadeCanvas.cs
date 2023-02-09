using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    private UIFade _uiFade;

    public bool IsFadeOutFinished { get => _uiFade.IsFadeOutFinished; }
    
    public static FadeCanvas Instance { get; set; }
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

        _uiFade = GetComponentInChildren<UIFade>();
    }

    public void FadeIn()
    {
        _uiFade.FadeIn();
    }

    public void FadeOut()
    {
        _uiFade.FadeOut();
    }
}
