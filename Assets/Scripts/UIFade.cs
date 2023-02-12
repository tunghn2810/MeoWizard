using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    private Animator _anim;

    private bool _isFadeOutFinished = false;
    public bool IsFadeOutFinished { get => _isFadeOutFinished; }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void FinishFadingOut()
    {
        _isFadeOutFinished = true;
        SceneLoader.I_SceneLoader.LoaderCallback();
    }

    public void FadeIn()
    {
        _anim.SetBool("isFadeOut", false);
        _isFadeOutFinished = false;
    }

    public void FadeOut()
    {
        _anim.SetBool("isFadeOut", true);
    }
}
