using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Bomb _bomb;

    private void Awake()
    {
        _bomb = GetComponentInParent<Bomb>();
    }

    public void EndExplode()
    {
        _bomb.IsExplodeEnd = true;
    }
}
