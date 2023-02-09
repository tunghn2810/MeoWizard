using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private bool _hasBomb = false;
    [SerializeField] private List<GameObject> players;
    public List<GameObject> Players { get => players; set => players = value; }

    private void SpawnBomb(object sender, PlayerFunctions.OnPlantBombEventargs e)
    {
        if (!_hasBomb)
        {
            Vector3 bombPosition = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject newBomb = Instantiate(_bombPrefab, bombPosition, Quaternion.identity);
            newBomb.GetComponent<Bomb>().Power = e.bombPower;
            newBomb.GetComponent<Bomb>().Player = e.player;
            newBomb.GetComponent<Bomb>().IsOneShot = e.player.IsOnFire;
            e.player.AddBomb(newBomb);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            _hasBomb = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            players.Add(collision.gameObject);
            collision.gameObject.GetComponent<PlayerFunctions>().OnPlantBomb += SpawnBomb;
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
            collision.gameObject.GetComponent<PlayerFunctions>().OnPlantBomb -= SpawnBomb;
        }
    }
}
