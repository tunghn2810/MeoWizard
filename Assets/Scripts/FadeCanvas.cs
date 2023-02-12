using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    private UIFade _uiFade;

    public bool IsFadeOutFinished { get => _uiFade.IsFadeOutFinished; }
    
    public static FadeCanvas I_FadeCanvas { get; set; }
    private void Awake()
    {
        if (I_FadeCanvas == null)
        {
            I_FadeCanvas = this;
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
