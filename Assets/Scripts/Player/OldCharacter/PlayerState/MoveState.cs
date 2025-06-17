using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState :  IState
{
    private PlayerBehaviour _playerBehaviour;
    private StateMachine _stateMachine;
    public MoveState(PlayerBehaviour playerBehaviour, StateMachine stateMachine)
    {
        this._playerBehaviour = playerBehaviour;
        this._stateMachine = stateMachine;

    }

    public void Enter()
    {
        Debug.Log("Entraste al estado : movimiento");
        SFXManager.Instance.PlayLoop(_playerBehaviour.StepsSFX);
    }

    public void Exit()
    {
        Debug.Log("saliste del estado: movimiento");
        SFXManager.Instance.StopLoop();
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
       _playerBehaviour.SetMovementInput(input);
        if (input.magnitude <= 0.01f)
        {
            _stateMachine.TransitionTo(_stateMachine.idleState);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _stateMachine.TransitionTo(_stateMachine.impulserState); //Pasaje a estado de impulso
        }

    }
}
