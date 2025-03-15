using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    public static bool isSwingTriggered { get; private set; }
    public static bool isTransformTriggered { get; private set; }
    public static bool isInteractTriggered { get; private set; }
    public static bool isAbilityTriggered { get; private set; }
    public static bool isComboTriggered { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _swingAction;
    private InputAction _transformAction;
    private InputAction _interactAction;
    private InputAction _abilityAction;
    private InputAction _comboAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _swingAction = _playerInput.actions["Swing"];
        _transformAction = _playerInput.actions["Transform"];
        _interactAction = _playerInput.actions["Interact"];
        _abilityAction = _playerInput.actions["Ability"];
        _comboAction = _playerInput.actions["Combo"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        isSwingTriggered = _swingAction.triggered;
        isTransformTriggered = _transformAction.triggered;
        isInteractTriggered = _interactAction.triggered;
        isAbilityTriggered = _abilityAction.triggered;
        isComboTriggered = _comboAction.triggered;
    }
}
