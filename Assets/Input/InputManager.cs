using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    public static bool isSwingTriggered { get; private set; }
    public static bool isTransformTriggered { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _swingAction;
    private InputAction _transformAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _swingAction = _playerInput.actions["Swing"];
        _transformAction = _playerInput.actions["Transform"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        isSwingTriggered = _swingAction.triggered;
        isTransformTriggered = _transformAction.triggered;
    }
}
