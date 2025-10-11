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

    public event Action SkipDialoguePressed;
    public event Action SkipDialogueReleased;
    public event Action SkipDialogueHeld;

    private bool inputsLocked = false;
    private bool dialogueInputsLocked = false;

    
    public void LockInputs()
    {
        inputsLocked = true;
        Move?.Invoke(Vector2.zero);
        Look?.Invoke(Vector2.zero);
    }

    public void UnlockInputs()
    {
        inputsLocked = false;
    }

    public void LockDialogueInputs()
    {
        dialogueInputsLocked = true;
    }

    public void UnlockDialogueInputs()
    {
        dialogueInputsLocked = false;
    }

    public bool AreInputsLocked()
    {
        return inputsLocked;
    }

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
        inputActions.Player.Move.performed += ctx => { if (!inputsLocked) Move?.Invoke(ctx.ReadValue<Vector2>()); };
        inputActions.Player.Move.canceled += ctx => {  Move?.Invoke(Vector2.zero); };

        // --- Look ---
        inputActions.Player.Look.performed += ctx => { if (!inputsLocked) Look?.Invoke(ctx.ReadValue<Vector2>()); };
        inputActions.Player.Look.canceled += ctx => {  Look?.Invoke(Vector2.zero); };

        // --- Pause ---
        inputActions.Player.Pause.started += ctx => PausePressed?.Invoke();
        inputActions.Player.Pause.canceled += ctx => PauseReleased?.Invoke();

        // --- Skip Dialogue ---
        inputActions.UI.Submit.started += ctx => { if (!dialogueInputsLocked) SkipDialoguePressed?.Invoke(); };
        inputActions.UI.Submit.performed += ctx => { if (!dialogueInputsLocked) SkipDialogueHeld?.Invoke(); };
        inputActions.UI.Submit.canceled += ctx => SkipDialogueReleased?.Invoke();

        // --- Interact ---
        inputActions.Player.Interact.started += ctx => { if (!inputsLocked) InteractPressed?.Invoke(); };
        inputActions.Player.Interact.performed += ctx => { if (!inputsLocked) InteractHeld?.Invoke(); };
        inputActions.Player.Interact.canceled += ctx => {  InteractReleased?.Invoke(); };

        // --- Change Character ---
        inputActions.Player.ChangeCharacter.started += ctx => { if (!inputsLocked) ChangeCharacterPressed?.Invoke(); };
        inputActions.Player.ChangeCharacter.canceled += ctx => {  ChangeCharacterReleased?.Invoke(); };

        // --- Action ---
        inputActions.Player.Action.started += ctx => { if (!inputsLocked) ActionPressed?.Invoke(); };
        inputActions.Player.Action.performed += ctx => { if (!inputsLocked) ActionHeld?.Invoke(); };
        inputActions.Player.Action.canceled += ctx => { ActionReleased?.Invoke(); };

        // --- Special Action ---
        inputActions.Player.SpecialAction.started += ctx => { if (!inputsLocked) SpecialActionPressed?.Invoke(); };
        inputActions.Player.SpecialAction.performed += ctx => { if (!inputsLocked) SpecialActionHeld?.Invoke(); };
        inputActions.Player.SpecialAction.canceled += ctx => {  SpecialActionReleased?.Invoke(); };
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
