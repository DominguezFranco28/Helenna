using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileIdleState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    public AgileIdleState(AgilePlayerBehaviour playerBehaviour, AgileStateMachine agileStateMachine )
    {
        this._agilePlayerBehaviour = playerBehaviour;
        this._agileStateMachine = agileStateMachine;
    }
    public void Enter()
    {
        Debug.Log("Hola soy el pj2");
        _agilePlayerBehaviour.SetMovementInput(Vector2.zero); //Freezeamos al pj al entrar a este estado.
        _agilePlayerBehaviour.SetMovementEnabled(true);

    }

    public void Exit()
    {

        Debug.Log("Saliste del estado: IDLE PERRO");
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (input.magnitude > 0.01f)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.moveState); //Pasaje a estado de movimiento
        }
    }
}
