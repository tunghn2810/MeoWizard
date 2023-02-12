using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public event Action OnSubmit;

    public void Submit()
    {
        OnSubmit?.Invoke();
    }
}
