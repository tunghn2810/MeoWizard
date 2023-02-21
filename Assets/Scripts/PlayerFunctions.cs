using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

using static LevelManager;

public class PlayerFunctions : MonoBehaviour
{
    //References
    private Rigidbody2D _rgbd;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private AnimatorOverrideController _onFireController;
    [SerializeField] private GameObject _bombPrefab;

    //Layers
    private LayerMask _obstacleLayer;
    private LayerMask _bombLayer;
    private LayerMask _directionCheckLayer;

    //Movement
    private Vector2 _moveDirection = Vector2.zero;
    [SerializeField] private float _moveSpeed = 0.064f;

    //For checking obstacle and sliding when bumping into one
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private Transform[] _outerRaycasts;
    [SerializeField] private Transform[] _innerRaycasts;
    private bool _canSlide;
    private bool _isBlocked;
    private bool _slideForward;
    private int _flip = 1;
    private bool _isBlockedByBomb;

    //For when player holds multiple keys at once
    private bool _currentlyHorz = false;
    private int _multiInput = 0;
    private List<int> _inputs = new List<int>();
    private int _currentInput; //0-1-2-3 = Up-Down-Left-Right
    private bool[] _canGoDirection = { true, true, true, true }; //Up Down Left Right

    //Movement states
    private bool _isMovingUp = false;
    private bool _isMovingDown = false;
    private bool _isMovingLeft = false;
    private bool _isMovingRight = false;

    //For bomb planting 
    private List<GameObject> _bombList = new List<GameObject>();
    private Tile _currentTile;

    //Iframes
    private float _iFrameTimer = 0;
    private const float IFRAME = 1f;
    private bool _isIFrame = false;

    //Afk timer
    private float _afkTimer = 0;
    private const float AFK_TIME = 3f;
    private bool _isAfk = false;

    //Stats
    [SerializeField] private int _playerNum;
    public int PlayerNum { get => _playerNum; set => _playerNum = value; }
    
    private int _playerScore;
    public int PlayerScore { get => _playerScore; set => _playerScore = value; }

    private int _power = 1;
    public int Power { get => _power; set => _power = value; }
    private const int MAX_POWER = 5;

    private int _bombCap = 2;
    public int BombCap { get => _bombCap; set => _bombCap = value; }
    private const int MAX_BOMBCAP = 10;

    private bool _isDead = false;
    private bool _isOnFire = false;
    public bool IsOnFire { get => _isOnFire; set => _isOnFire = value; }

    private void Start()
    {
        _rgbd = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _obstacleLayer = LayerMask.GetMask("Obstacle", "SoftWall");
        _bombLayer = LayerMask.GetMask("Bomb");
        _directionCheckLayer = LayerMask.GetMask("Obstacle", "Bomb", "SoftWall");
    }

    private void OnEnable()
    {
        I_LevelManager.OnWin += Win;
    }

    private void OnDisable()
    {
        I_LevelManager.OnWin -= Win;
    }

    private void Update()
    {
        if (_isAfk && !_isDead)
        {
            if (_afkTimer >= AFK_TIME)
            {
                _animator.SetBool("isAfk", _isAfk);
            }

            _afkTimer += Time.deltaTime;
        }
        else
        {
            _afkTimer = 0;
            _animator.SetBool("isAfk", _isAfk);
        }

        if (_isIFrame && !_isDead)
        {
            if (_iFrameTimer >= IFRAME)
            {
                _isIFrame = false;
                _iFrameTimer = 0;
            }

            _spriteRenderer.enabled = !_spriteRenderer.enabled;

            _iFrameTimer += Time.deltaTime;
        }
        else
            _spriteRenderer.enabled = true;
    }

    private void FixedUpdate()
    {
        if (_isDead)
            return;

        //Get current input from input queue
        if (_inputs.Count > 0)
            _currentInput = _inputs[0];
        else
            _currentInput = -1; //-1 = No input


        if (_currentInput == -1)
            _isAfk = true;
        else
            _isAfk = false;

        //Process input
        if (_currentInput == 0)
        {
            MoveInputProcess(Vector2.up);
            _animator.SetFloat("moveDirectionX", 0);
            _animator.SetFloat("moveDirectionY", 1);
        }
        if (_currentInput == 1)
        {
            MoveInputProcess(Vector2.down);
            _animator.SetFloat("moveDirectionX", 0);
            _animator.SetFloat("moveDirectionY", -1);
        }
        if (_currentInput == 2)
        {
            MoveInputProcess(Vector2.left);
            _animator.SetFloat("moveDirectionX", -1);
            _animator.SetFloat("moveDirectionY", 0);
        }
        if (_currentInput == 3)
        {
            MoveInputProcess(Vector2.right);
            _animator.SetFloat("moveDirectionX", 1);
            _animator.SetFloat("moveDirectionY", 0);
        }

        if (_inputs.Count == 0)
        {
            _moveDirection = Vector2.zero;
            _animator.SetBool("isWalking", false);
        }
        else
        {
            _animator.SetBool("isWalking", true);
        }

        //Move the player
        _rgbd.MovePosition(_rgbd.position + _moveDirection * _moveSpeed);
    }

    //Take move input from player
    public void MoveInput(int direction) //0-1-2-3 = Up-Down-Left-Right
    {
        if (_isDead)
            return;

        if (direction == 0)
        {
            _isMovingUp = !_isMovingUp;
            MoveInputQueue(_isMovingUp, direction);
        }
        if (direction == 1)
        {
            _isMovingDown = !_isMovingDown;
            MoveInputQueue(_isMovingDown, direction);
        }
        if (direction == 2)
        {
            _isMovingLeft = !_isMovingLeft;
            MoveInputQueue(_isMovingLeft, direction);
        }
        if (direction == 3)
        {
            _isMovingRight = !_isMovingRight;
            MoveInputQueue(_isMovingRight, direction);
        }
    }

    //Add and remove input from the move input queue
    private void MoveInputQueue(bool isMovingDirection, int actionNum)
    {
        if (isMovingDirection)
        {
            _multiInput += 1;
            _inputs.Add(actionNum);
        }
        else
        {
            _multiInput -= 1;
            _inputs.Remove(actionNum);
        }
    }

    //Process move input to determine the direction
    private void MoveInputProcess(Vector2 direction)
    {
        Vector2 checkDirection = direction.x == 0 ? Vector2.left : Vector2.up;
        Vector3 rotation = (direction == Vector2.up) || (direction == Vector2.left) ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180);
        int flip = rotation.z == 0 ? 1 : -1;

        ObstacleCheck(direction);
        BombCheck(direction);

        if (_isBlocked || _isBlockedByBomb)
            _moveDirection = Vector2.zero;
        else if (_canSlide)
            if (_slideForward)
                _moveDirection = _flip * checkDirection;
            else
                _moveDirection = _flip * -1f * checkDirection;
        else
        {
            _raycastOrigin.eulerAngles = rotation;
            _flip = flip;
            _moveDirection = direction;
        }
    }

    //Check for obstacle to determine whether to block or let player slide into the moving direction
    private void ObstacleCheck(Vector2 direction)
    {
        //Offset to differentiate horizontal and vertical checks
        int iOffset = 0; //Horizontal
        if (direction.x == 0)
            iOffset = 2; //Vertical

        //Checks for each raycast
        bool[] boolCheck = { false, false, false, false };
        int checkCount = 0;

        for (int i = iOffset; i < (iOffset + 2); i++)
        {
            RaycastHit2D outerTopHit = Physics2D.Raycast(_outerRaycasts[i].transform.position, direction, 0.6f, _obstacleLayer);
            if (outerTopHit)
            {
                boolCheck[checkCount] = true;
            }
            checkCount++;

            RaycastHit2D innerBottomHit = Physics2D.Raycast(_innerRaycasts[i].transform.position, direction, 0.6f, _obstacleLayer);
            if (innerBottomHit)
            {
                boolCheck[checkCount] = true;
            }
            checkCount++;
        }

        //Cases that can occur (0,3,1,2 - from top to bottom)
        if (boolCheck[0] == true)
        {
            if (boolCheck[3] == true)
            {
                _isBlocked = true;
            }
            else
            {
                _canSlide = true;
                _isBlocked = false;
                _slideForward = false;
            }
        }
        else
        {
            if (boolCheck[2] == true)
            {
                _canSlide = true;
                _isBlocked = false;
                _slideForward = true;
            }
            else
            {
                _canSlide = false;
                _isBlocked = false;
            }
        }
    }

    //Obstacle check, but for bombs
    private void BombCheck(Vector2 direction)
    {
        RaycastHit2D[] checkHits = Physics2D.RaycastAll(transform.position, direction, 0.6f, _bombLayer);
        if (checkHits.Length == 0)
            _isBlockedByBomb = false;

        for (int i = 0; i < checkHits.Length; i++)
        {
            if (checkHits[i].transform.GetComponent<Bomb>().PlayerCheck(gameObject))
                _isBlockedByBomb = true;
            else
                _isBlockedByBomb = false;
        }
    }

    //Check 4 directions around the player for obstacles
    private void Obstacle4DCheck()
    {
        //Horizontal checks
        for (int i = -1; i <= 1; i += 2)
        {
            RaycastHit2D obsCheck = Physics2D.Raycast(transform.position, Vector2.right * i, 1f, _directionCheckLayer);
            if (obsCheck.collider != null)
            {
                if (i == -1)
                    _canGoDirection[2] = false;
                else
                    _canGoDirection[3] = false;
            }
            else
            {
                if (i == -1)
                    _canGoDirection[2] = true;
                else
                    _canGoDirection[3] = true;
            }
        }

        //Vertical checks
        for (int i = -1; i <= 1; i += 2)
        {
            RaycastHit2D obsCheck = Physics2D.Raycast(transform.position, Vector2.up * i, 1f, _directionCheckLayer);
            if (obsCheck.collider != null)
            {
                if (i == -1)
                    _canGoDirection[1] = false;
                else
                    _canGoDirection[0] = false;
            }
            else
            {
                if (i == -1)
                    _canGoDirection[1] = true;
                else
                    _canGoDirection[0] = true;
            }
        }
    }

    //Move the top input back to the bottom of the movement input queue
    private void MoveInputBack()
    {
        int toMove = _inputs[0];
        _inputs.Remove(toMove);
        _inputs.Add(toMove);
    }

    public void PlantBomb()
    {
        if (_isDead)
            return;

        if (_bombList.Count < _bombCap)
            SpawnBomb();
    }

    private void SpawnBomb()
    {
        if (!_currentTile.HasBomb)
        {
            Vector3 bombPosition = new Vector3(_currentTile.transform.position.x, _currentTile.transform.position.y, 0);
            GameObject newBomb = Instantiate(_bombPrefab, bombPosition, Quaternion.identity);
            newBomb.GetComponent<Bomb>().Power = _power;
            newBomb.GetComponent<Bomb>().Player = this;
            newBomb.GetComponent<Bomb>().IsOneShot = _isOnFire;
            AddBomb(newBomb);
        }
    }

    public void AddBomb(GameObject bomb)
    {
        _bombList.Add(bomb);
    }

    public void DeleteBomb(GameObject bomb)
    {
        _bombList.Remove(bomb);
    }

    public void IncreaseBombCap()
    {
        if (_bombCap < MAX_BOMBCAP)
            _bombCap += 1;
    }

    public void IncreasePower()
    {
        if (_power < MAX_POWER)
            _power += 1;
    }
    
    public void OnFire()
    {
        _isIFrame = true;
        _isOnFire = true;
        _animator.runtimeAnimatorController = _onFireController;
        Invoke("Die", 10.0f);
        _power = MAX_POWER;
        _moveSpeed *= 1.2f;
    }

    public void Die()
    {
        if (_isDead)
            return;
        
        _isIFrame = true;
        _animator.SetBool("isDead", true);
        _isDead = true;

        I_LevelManager.RemovePlayer(_playerNum);
    }

    public void Win()
    {
        if (!_isDead)
            _animator.SetBool("isWin", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check when entering a new tile
        if (collision.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            _currentTile = collision.gameObject.GetComponent<Tile>();

            //Check if currently moving horizontally or vertically
            if (_moveDirection.x == 0)
                _currentlyHorz = false;
            else
                _currentlyHorz = true;

            //Check for surrounding obstacles
            Obstacle4DCheck();

            //Check if player is holding multiple move input
            if (_multiInput > 1 && _canGoDirection.Contains(true))
            {
                if (_currentlyHorz && (_canGoDirection[0] || _canGoDirection[1]) && (_currentInput != 0 || _currentInput != 1))
                {
                    MoveInputBack();
                }
                else if (!_currentlyHorz && (_canGoDirection[2] || _canGoDirection[3]) && (_currentInput != 0 || _currentInput != 1)) //CHECK THIS
                {
                    MoveInputBack();
                }
            }
        }

        //Check when getting hit by fire
        if (!_isIFrame && !_isDead)
        {
            if (collision.gameObject.tag == "Fire")
            {
                if (_isOnFire)
                    Die();
                else
                    OnFire();
            }
            else if (collision.gameObject.tag == "FireOneShot")
            {
                Die();
            }
        }
    }
}
