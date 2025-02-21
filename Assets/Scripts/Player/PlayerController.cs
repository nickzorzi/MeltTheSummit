using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
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
    [SerializeField] private int coolCost;
    [SerializeField] private int burn;

    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private bool _isTransformed = false;
    [SerializeField] private bool _isBurning = false;

    private bool _inCoroutine = false;

    private Vector2 _movement;

    private Rigidbody2D _rb;
    private Animator _animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        _rb.velocity = _movement * _moveSpeed;

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

        HandleTemp();
        HandleHealth();
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

        yield return new WaitForSeconds(1);

        _inCoroutine = false;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Projectile"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        OnPlayerDamaged?.Invoke();
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
