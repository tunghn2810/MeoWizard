using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftWall : MonoBehaviour
{
    private Animator _anim;
    private GameObject _item;

    private bool _isDestroyed = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (_isDestroyed)
            Destroy(gameObject);
    }

    public void AddItem(GameObject item)
    {
        _item = item;
    }

    public void Destroyed()
    {
        _anim.SetBool("isDestroyed", true);
    }

    public void EndDestroyed()
    {
        if (_item)
            _item.GetComponent<Item>().Appear();
        _isDestroyed = true;
    }
}
