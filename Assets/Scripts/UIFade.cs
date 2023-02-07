using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        _anim.SetBool("isFadeOut", false);
    }

    public void FadeOut()
    {
        _anim.SetBool("isFadeOut", true);
    }
}
