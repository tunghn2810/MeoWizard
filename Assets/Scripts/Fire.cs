using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Bomb bomb;

    private void Awake()
    {
        bomb = GetComponentInParent<Bomb>();
    }

    public void EndExplode()
    {
        bomb.IsExplodeEnd = true;
    }
}
