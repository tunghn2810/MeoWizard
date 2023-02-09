using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Transform _tile;
    [SerializeField] private List<GameObject> _remainPlayers;

    private SpriteRenderer _bombSprite;
    private Collider2D _bombCollider;

    private float _timer;
    [SerializeField] private GameObject _fireCenter;
    [SerializeField] private GameObject _fireUp;
    [SerializeField] private GameObject _fireDown;
    [SerializeField] private GameObject _fireLeft;
    [SerializeField] private GameObject _fireRight;
    private LayerMask _directionCheckLayer;

    private int _power;
    public int Power { get => _power; set => _power = value; }

    private const float BOMB_TIMER = 2f;

    private bool _isExplodeEnd = false;
    public bool IsExplodeEnd { get => _isExplodeEnd; set => _isExplodeEnd = value; }

    private bool _isTriggered = false;
    public bool IsTriggered { get => _isTriggered; set => _isTriggered = value; }

    private bool _isOneShot = false;
    public bool IsOneShot { get => _isOneShot; set => _isOneShot = value; }

    private PlayerFunctions _player;
    public PlayerFunctions Player { get => _player; set => _player = value; }

    private void Awake()
    {
        _bombSprite = GetComponent<SpriteRenderer>();
        _bombCollider = GetComponent<BoxCollider2D>();

        _directionCheckLayer = LayerMask.GetMask("Obstacle", "Bomb", "FireCenter", "SoftWall", "Item");
    }

    private void Start()
    {
        _isOneShot = _player.IsOnFire;
        SetBombTimer(BOMB_TIMER);
    }

    private void SetBombTimer(float timer)
    {
        _timer = timer;
    }

    private void Update()
    {
        //Bomb trigger
        if (_isTriggered)
            ExplodeTrigger();
        else
        {
            //Bomb timer
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0f)
                {
                    Explode();
                }
            }
        }

        if (_tile != null)
            _remainPlayers = _tile.GetComponent<Tile>().Players;

        if (_isExplodeEnd)
        {
            Destroy(gameObject);
        }
    }

    //Bomb explode
    private void Explode()
    {
        _player.DeleteBomb(gameObject);

        _bombSprite.enabled = false;
        _bombCollider.enabled = false;

        if (_isOneShot)
            _fireCenter.tag = "FireOneShot";

        RaycastHit2D obsCheckUp = Physics2D.Raycast(transform.position, Vector2.up, _power + 1, _directionCheckLayer);
        if (obsCheckUp)
        {
            float distance = Mathf.Abs(obsCheckUp.transform.position.y - transform.position.y);
            if (distance > 1)
            {
                for (int i = 1; i < distance; i++)
                {
                    if (_isOneShot)
                        _fireUp.transform.GetChild(i).gameObject.tag = "FireOneShot";
                    _fireUp.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            //Fire into another bomb
            if (obsCheckUp.transform.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            {
                obsCheckUp.transform.gameObject.GetComponent<Bomb>().IsTriggered = true;
            }
            //Fire into a soft wall
            if (obsCheckUp.transform.gameObject.layer == LayerMask.NameToLayer("SoftWall"))
            {
                obsCheckUp.transform.gameObject.GetComponent<SoftWall>().Destroyed();
            }
            //Fire into an item
            if (obsCheckUp.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                obsCheckUp.transform.gameObject.GetComponent<Item>().Destroyed();
            }
        }
        else
        {
            for (int i = 1; i <= _power; i++)
            {
                if (_isOneShot)
                    _fireUp.transform.GetChild(i).gameObject.tag = "FireOneShot";
                _fireUp.transform.GetChild(i).gameObject.SetActive(true);
            }
            _fireUp.transform.GetChild(0).gameObject.SetActive(true);
            _fireUp.transform.GetChild(0).position += new Vector3(0, _power, 0);
        }

        RaycastHit2D obsCheckDown = Physics2D.Raycast(transform.position, Vector2.down, _power + 1, _directionCheckLayer);
        if (obsCheckDown)
        {
            float distance = Mathf.Abs(obsCheckDown.transform.position.y - transform.position.y);
            if (distance > 1)
            {
                for (int i = 1; i < distance; i++)
                {
                    if (_isOneShot)
                        _fireDown.transform.GetChild(i).gameObject.tag = "FireOneShot";
                    _fireDown.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            //Fire into another bomb
            if (obsCheckDown.transform.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            {
                obsCheckDown.transform.gameObject.GetComponent<Bomb>().IsTriggered = true;
            }
            //Fire into a soft wall
            if (obsCheckDown.transform.gameObject.layer == LayerMask.NameToLayer("SoftWall"))
            {
                obsCheckDown.transform.gameObject.GetComponent<SoftWall>().Destroyed();
            }
            //Fire into an item
            if (obsCheckDown.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                obsCheckDown.transform.gameObject.GetComponent<Item>().Destroyed();
            }
        }
        else
        {
            for (int i = 1; i <= _power; i++)
            {
                if (_isOneShot)
                    _fireDown.transform.GetChild(i).gameObject.tag = "FireOneShot";
                _fireDown.transform.GetChild(i).gameObject.SetActive(true);
            }
            _fireDown.transform.GetChild(0).gameObject.SetActive(true);
            _fireDown.transform.GetChild(0).position -= new Vector3(0, _power, 0);
        }

        RaycastHit2D obsCheckLeft = Physics2D.Raycast(transform.position, Vector2.left, _power + 1, _directionCheckLayer);
        if (obsCheckLeft)
        {
            float distance = Mathf.Abs(obsCheckLeft.collider.transform.position.x - transform.position.x);
            if (distance > 1)
            {
                for (int i = 1; i < distance; i++)
                {
                    if (_isOneShot)
                        _fireLeft.transform.GetChild(i).gameObject.tag = "FireOneShot";
                    _fireLeft.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            //Fire into another bomb
            if (obsCheckLeft.transform.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            {
                obsCheckLeft.transform.gameObject.GetComponent<Bomb>().IsTriggered = true;
            }
            //Fire into a soft wall
            if (obsCheckLeft.transform.gameObject.layer == LayerMask.NameToLayer("SoftWall"))
            {
                obsCheckLeft.transform.gameObject.GetComponent<SoftWall>().Destroyed();
            }
            //Fire into an item
            if (obsCheckLeft.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                obsCheckLeft.transform.gameObject.GetComponent<Item>().Destroyed();
            }
        }
        else
        {
            for (int i = 1; i <= _power; i++)
            {
                if (_isOneShot)
                    _fireLeft.transform.GetChild(i).gameObject.tag = "FireOneShot";
                _fireLeft.transform.GetChild(i).gameObject.SetActive(true);
            }
            _fireLeft.transform.GetChild(0).gameObject.SetActive(true);
            _fireLeft.transform.GetChild(0).position -= new Vector3(_power, 0, 0);
        }

        RaycastHit2D obsCheckRight = Physics2D.Raycast(transform.position, Vector2.right, _power + 1, _directionCheckLayer);
        if (obsCheckRight)
        {
            float distance = Mathf.Abs(obsCheckRight.collider.transform.position.x - transform.position.x);
            if (distance > 1)
            {
                for (int i = 1; i < distance; i++)
                {
                    if (_isOneShot)
                        _fireRight.transform.GetChild(i).gameObject.tag = "FireOneShot";
                    _fireRight.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            //Fire into another bomb
            if (obsCheckRight.transform.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            {
                obsCheckRight.transform.gameObject.GetComponent<Bomb>().IsTriggered = true;
            }
            //Fire into a soft wall
            if (obsCheckRight.transform.gameObject.layer == LayerMask.NameToLayer("SoftWall"))
            {
                obsCheckRight.transform.gameObject.GetComponent<SoftWall>().Destroyed();
            }
            //Fire into an item
            if (obsCheckRight.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                obsCheckRight.transform.gameObject.GetComponent<Item>().Destroyed();
            }
        }
        else
        {
            for (int i = 1; i <= _power; i++)
            {
                if (_isOneShot)
                    _fireRight.transform.GetChild(i).gameObject.tag = "FireOneShot";
                _fireRight.transform.GetChild(i).gameObject.SetActive(true);
            }
            _fireRight.transform.GetChild(0).gameObject.SetActive(true);
            _fireRight.transform.GetChild(0).position += new Vector3(_power, 0, 0);
        }

        _fireCenter.SetActive(true);
    }

    private void ExplodeTrigger()
    {
        Invoke("Explode", 0.1f);
    }

    //Collision checking with players
    public bool PlayerCheck(GameObject playerToCheck)
    {
        if (_remainPlayers.Contains(playerToCheck))
            return false;
        else
            return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            _tile = collision.transform;
        }
    }
}


