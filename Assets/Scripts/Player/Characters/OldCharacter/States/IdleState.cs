using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;

    //Constructor, because it does not inherit from monobehaviour
    public IdleState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine) 
    {
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._oldStateMachine = oldStateMachine;
    }
    public void Enter()
    {
        Debug.Log("You entered the state: OLD IDLE");
        _oldPlayerBehaviour.SetMovementInput(Vector2.zero); 
    }

    public void Exit()
    {
        Debug.Log("You left the state: OLD IDLE");
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.magnitude > 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.moveState); 
        }

        _oldPlayerBehaviour.SetMovementInput(Vector2.zero);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.impulseState); 
        }
    }
}

