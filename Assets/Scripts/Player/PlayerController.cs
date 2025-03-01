using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerThermo;

    [SerializeField] private float _moveSpeed = 5f;
    public float health = 12;
    public float maxHealth = 12;
    public float temp, maxTemp;
    [SerializeField] private int heatCost;
    public int coolCost;
    [SerializeField] private int burn;

    public bool _canAttack = true;
    public bool _firstLoad = false;
    public bool _healthUpdate = false;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private bool _isTransformed = false;
    [SerializeField] private bool _isBurning = false;

    public int flowers = 0;

    private bool _inCoroutine = false;

    private Vector2 _movement;

    private Rigidbody2D _rb;
    private Animator _animator;

    public bool _canSlide = false;
    [SerializeField] private Vector2 _slideDirection = Vector2.zero;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform rayPoint;


    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    public bool isInv = false;

    [SerializeField] private HitFlash flashEffect;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<HitFlash>();
    }

    private void OnEnable()
    {
        if (_firstLoad)
        {
            return;
        }
        else
        {
            health = PlayerData.Instance.health;
            coolCost = PlayerData.Instance.coolCost;
            flowers = PlayerData.Instance.flowers;
            temp = PlayerData.Instance.temp;
            _isTransformed = PlayerData.Instance._isTransformed;
            _isBurning = PlayerData.Instance._isBurning;

            OnPlayerDamaged?.Invoke();

            _firstLoad = false;
        }
    }

    private void OnDisable()
    {
        PlayerData.Instance.health = health;
        PlayerData.Instance.coolCost = coolCost;
        PlayerData.Instance.flowers = flowers;
        PlayerData.Instance.temp = temp;
        PlayerData.Instance._isTransformed = _isTransformed;
        PlayerData.Instance._isBurning = _isBurning;
    }

    private void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        if (_canSlide)
        {
            if (_slideDirection == Vector2.zero && _movement != Vector2.zero)
            {
                if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
                {
                    _slideDirection = new Vector2(Mathf.Sign(_movement.x), 0);
                }
                else
                {
                    _slideDirection = new Vector2(0, Mathf.Sign(_movement.y));
                }
            }

            if (_slideDirection != Vector2.zero)
            {
                _moveSpeed = 8;
                _rb.velocity = _slideDirection * _moveSpeed;

                if (IsCollidingWithWall())
                {
                    _rb.velocity = Vector2.zero;
                    _slideDirection = Vector2.zero;
                    _moveSpeed = 5;

                    _rb.velocity = _movement * _moveSpeed;
                }
            }
        }
        else
        {
            _rb.velocity = _movement * _moveSpeed;

            _slideDirection = Vector2.zero;
        }

        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (!_isAttacking)
        {
            if (_movement != Vector2.zero)
            {
                _animator.SetFloat(_lastHorizontal, _movement.x);
                _animator.SetFloat(_lastVertical, _movement.y);
            }
        }

        HandleTemp();

        HandleHealth();

        if (!_canAttack)
        {
            return;
        }

        if (InputManager.isTransformTriggered && !_isTransformed && !_isAttacking)
        {
            _isTransformed = true;
            StartCoroutine(Transform("In"));
        }
        else if (InputManager.isTransformTriggered && _isTransformed && !_isAttacking)
        {
            _isTransformed = false;
            StartCoroutine(Transform("Out"));
        }

        if (InputManager.isSwingTriggered && !_isAttacking)
        {
            if (!_isTransformed)
            {
                StartCoroutine(Swing("D", 0.8f));
            }
            else if (_isTransformed)
            {
                StartCoroutine(Swing("A", 0.4f));
            }
        }
    }

    private bool IsCollidingWithWall()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, _slideDirection, 0.25f, wallLayer);
        //Collider2D hit = Physics2D.OverlapCircle(rayPoint.position, 0.41f, wallLayer);

        float boxWidth = 0.4f;
        float boxHeight = 0.2f;
        float offsetDistance = 0.3f;
        Vector2 boxCenter = (Vector2)rayPoint.position + _slideDirection * offsetDistance;
        Collider2D hit = Physics2D.OverlapBox(boxCenter, new Vector2(boxWidth, boxHeight), 0f, wallLayer);

        if (hit != null && hit.CompareTag("Wall")) //hit.collider != null && hit.collider.CompareTag("Wall)"
        {
            return true;
        }

        return false;
    }

    private IEnumerator Invulnerability()
    {
        isInv = true;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 1, 1, 0.25f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        isInv = false;
    }

    IEnumerator Swing(string mode, float time)
    {
        _animator.SetTrigger("Swing-" + mode);
        _isAttacking = true;
        _moveSpeed = 0f;
        yield return new WaitForSeconds(time);
        _isAttacking = false;
        _moveSpeed = 5f;
    }

    IEnumerator Transform(string mode)
    {
        _animator.SetTrigger("Transform-" + mode);
        _isAttacking = true;
        _moveSpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        _moveSpeed = 5f;
    }

    private void HandleHealth()
    {
        if (_isBurning && !_inCoroutine)
        {
            StartCoroutine(Burn(burn));
        }
        
        if (health <= 0)
        {
            Die();
        }
        else if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    private void HandleTemp()
    {
        if (_isTransformed)
        {
            StartCoroutine(ChangeTemp(heatCost, 0.1f));

            if (temp >= maxTemp)
            {
                temp = maxTemp;
                _isBurning = true;
            }
        }
        else
        {
            StartCoroutine(ChangeTemp(coolCost, 0.1f));
            _isBurning = false;

            if (temp <= 0)
            {
                temp = 0;
            }
        }
    }

    IEnumerator ChangeTemp(int amount, float tickSpeed)
    {
        yield return new WaitForSeconds(tickSpeed);

        temp += amount * Time.deltaTime;
        temp = Mathf.Clamp(temp, 0, maxTemp);

        OnPlayerThermo?.Invoke();
    }

    IEnumerator Burn(int amount)
    {
        _inCoroutine = true;

        TakeDamage(amount);

        flashEffect.Flash();

        yield return new WaitForSeconds(1);

        _inCoroutine = false;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Projectile"))
        {
            if (!isInv)
            {
                TakeDamage(1);

                StartCoroutine(Invulnerability());
            }
        }

        if (hitInfo.CompareTag("Ice"))
        {
            _canSlide = true;
        }

    }

    void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Ice"))
        {
            _canSlide = false;

            _moveSpeed = 5;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        OnPlayerDamaged?.Invoke();
    }

    public void GainHealth(float amount)
    {
        health += amount;
        OnPlayerDamaged?.Invoke();
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
