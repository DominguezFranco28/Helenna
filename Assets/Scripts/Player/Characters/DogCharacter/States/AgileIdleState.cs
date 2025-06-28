using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileIdleState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    public AgileIdleState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine )
    {
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
    }
    public void Enter()
    {
        Debug.Log("You entered the state: AGILE IDLE");
        _agilePlayerBehaviour.SetMovementInput(Vector2.zero); 
        _agilePlayerBehaviour.SetMovementEnabled(true);

    }
    public void Exit()
    {
        Debug.Log("You left the state: AGILE IDLE");
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
