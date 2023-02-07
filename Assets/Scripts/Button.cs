using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent OnSubmit;

    public void Submit()
    {
        OnSubmit?.Invoke();
    }
}