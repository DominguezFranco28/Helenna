using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileMoveState : IState
{
    private AgilePlayerBehaviour _playerBehaviour;
    private AgileStateMachine _agileStateMachine;
    public AgileMoveState(AgilePlayerBehaviour playerBehaviour, AgileStateMachine agileStateMachine)
    {
        this._playerBehaviour = playerBehaviour;
        this._agileStateMachine = agileStateMachine;
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
        _playerBehaviour.SetMovementInput(input);
        if (input.magnitude <= 0.01f)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
        }

    }
}
