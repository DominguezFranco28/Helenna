using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState :  IState
{
    private PlayerBehaviour _playerMovement;
    private StateMachine _stateMachine;
    public MoveState(PlayerBehaviour player,StateMachine stateMachine)
    {
        this._playerMovement = player;
        this._stateMachine = stateMachine;

    }

    public void Enter()
    {
        Debug.Log("Entraste al estado : movimiento");
    }

    public void Exit()
    {
        Debug.Log("saliste del estado: movimiento");
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
       _playerMovement.SetMovementInput(input);
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
