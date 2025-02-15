using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private bool _isAttacking = false;
    private bool _isTransformed = false;

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
        else
        {

        }

        if (InputManager.isTransformTriggered && !_isTransformed)
        {
            _isTransformed = true;
            StartCoroutine(Transform("In"));
        }
        else if (InputManager.isTransformTriggered && _isTransformed)
        {
            _isTransformed = false;
            StartCoroutine(Transform("Out"));
        }

        if (InputManager.isSwingTriggered && !_isAttacking)
        {
            if (!_isTransformed)
            {
                StartCoroutine(Swing("D", 1));
            }
            else if (_isTransformed)
            {
                StartCoroutine(Swing("A", 1));
            }
        }
    }

    IEnumerator Swing(string mode, int time)
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
}
