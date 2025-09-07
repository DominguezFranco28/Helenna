using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;

    private bool subbed = false;

    private void OnSpecialAction()
    {
        _oldPlayerBehaviour.LastMovementInput = _oldPlayerBehaviour.MovementInput;
        _oldStateMachine.TransitionTo(_oldStateMachine.impulseState);
    }
    private void OnMove(Vector2 movement)
    {
        if (movement.magnitude > 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.moveState);
            _oldPlayerBehaviour.SetMovementInput(movement);
        }
    }

    //Constructor, because it does not inherit from monobehaviour
    public IdleState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine) 
    {
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._oldStateMachine = oldStateMachine;
    }
    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;

                InputManager.Instance.SpecialActionPressed += OnSpecialAction;
                InputManager.Instance.Move += OnMove;
            }
        }
        
        Debug.Log("You entered the state: OLD IDLE");
        _oldPlayerBehaviour.SetMovementInput(Vector2.zero);
        _oldPlayerBehaviour.SetMovementEnabled(true);
    }

    public void Exit()
    {
        Debug.Log("You left the state: OLD IDLE");
        //desuscripcion del estado move porque daba problema con el impulse.
        if (InputManager.Instance != null)
        {
            InputManager.Instance.SpecialActionPressed -= OnSpecialAction;
            InputManager.Instance.Move -= OnMove;
            subbed = false;
        }
    }

    public void Update()
    {

    }
}

