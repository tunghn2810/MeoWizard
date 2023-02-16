using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private bool _hasBomb = false;
    public bool HasBomb { get => _hasBomb; set => _hasBomb = value; }
    [SerializeField] private List<GameObject> players;
    public List<GameObject> Players { get => players; set => players = value; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            _hasBomb = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            players.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            _hasBomb = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            players.Remove(collision.gameObject);
        }
    }
}
