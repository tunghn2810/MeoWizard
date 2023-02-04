using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Item : MonoBehaviour
{
    private Animator _anim;
    private GameObject _type;
    private GameObject _ring;

    private bool _isDestroyed = false;

    //Bomb types: 0 = Bomb, 1 = Fire
    private int _bombType;
    public int BombType { get => _bombType; }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _type = transform.GetChild(0).gameObject;
        _ring = transform.GetChild(1).gameObject;

        if (_type.GetComponent<SpriteRenderer>().sprite.name == "Item_Bomb")
            _bombType = 0;
        else if (_type.GetComponent<SpriteRenderer>().sprite.name == "Item_Fire")
            _bombType = 1;

        _type.GetComponent<SpriteRenderer>().enabled = false;
        _ring.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Update()
    {
        if (_isDestroyed)
            Destroy(gameObject);
    }

    public void Destroyed()
    {
        _type.SetActive(false);
        _ring.SetActive(false);
        _anim.SetBool("isDestroyed", true);
    }

    public void EndDestroyed()
    {
        _isDestroyed = true;
    }

    public void Appear()
    {
        _type.GetComponent<SpriteRenderer>().enabled = true;
        _ring.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (_bombType == 0)
            {
                collision.gameObject.GetComponent<PlayerMovement>().IncreaseBombCap();
            }
            else if (_bombType == 1)
            {
                collision.gameObject.GetComponent<PlayerMovement>().IncreasePowerp();
            }

            EndDestroyed();
        }
    }
}