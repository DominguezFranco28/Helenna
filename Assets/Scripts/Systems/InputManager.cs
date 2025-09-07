using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance => _instance;
    private static InputManager _instance;

    private InputSystem_Actions inputActions;

    // --- Events for gameplay code ---
    public event Action<Vector2> Move;
    public event Action<Vector2> Look;

    public event Action PausePressed;
    public event Action PauseReleased;

    public event Action InteractPressed;
    public event Action InteractReleased;
    public event Action InteractHeld;

    public event Action ChangeCharacterPressed;
    public event Action ChangeCharacterReleased;

    public event Action ActionPressed;
    public event Action ActionReleased;
    public event Action ActionHeld;

    public event Action SpecialActionPressed;
    public event Action SpecialActionReleased;
    public event Action SpecialActionHeld;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new InputSystem_Actions();
    }


    private void OnEnable()
    {
        inputActions.Enable();

        // --- Movement ---
        inputActions.Player.Move.performed += ctx => Move?.Invoke(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled += ctx => Move?.Invoke(Vector2.zero);

        // --- Look ---
        inputActions.Player.Look.performed += ctx => Look?.Invoke(ctx.ReadValue<Vector2>());
        inputActions.Player.Look.canceled += ctx => Look?.Invoke(Vector2.zero);

        // --- Pause ---
        inputActions.Player.Pause.started += ctx => PausePressed?.Invoke();
        inputActions.Player.Pause.canceled += ctx => PauseReleased?.Invoke();

        // --- Interact ---
        inputActions.Player.Interact.started += ctx => InteractPressed?.Invoke();
        inputActions.Player.Interact.performed += ctx => InteractHeld?.Invoke();
        inputActions.Player.Interact.canceled += ctx => InteractReleased?.Invoke();

        // --- Change Character ---
        inputActions.Player.ChangeCharacter.started += ctx => ChangeCharacterPressed?.Invoke();
        inputActions.Player.ChangeCharacter.canceled += ctx => ChangeCharacterReleased?.Invoke();

        // --- Action ---
        inputActions.Player.Action.started += ctx => ActionPressed?.Invoke();
        inputActions.Player.Action.performed += ctx => ActionHeld?.Invoke();
        inputActions.Player.Action.canceled += ctx => ActionReleased?.Invoke();

        // --- Special Action ---
        inputActions.Player.SpecialAction.started += ctx => SpecialActionPressed?.Invoke();
        inputActions.Player.SpecialAction.performed += ctx => SpecialActionHeld?.Invoke();
        inputActions.Player.SpecialAction.canceled += ctx => SpecialActionReleased?.Invoke();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void InvokeAction(Action action, float delay)
    {
        StartCoroutine(InvokeCoroutine(action, delay));
    }

    private IEnumerator InvokeCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public InputSystem_Actions GetInputSystem()
    {
        return inputActions;
}
}
