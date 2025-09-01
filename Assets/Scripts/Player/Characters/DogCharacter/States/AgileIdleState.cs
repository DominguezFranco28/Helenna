using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileIdleState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;

    private bool subbed = false;

    public AgileIdleState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine )
    {
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
    }
    private void OnMove(Vector2 movement)
    {
        if (movement.magnitude > 0.01f)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.moveState); //Pasaje a estado de movimiento
        }
    }

    public void Enter()
    {
        Debug.Log("You entered the state: AGILE IDLE");
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.Move += OnMove;
            }
        }
        
        _agilePlayerBehaviour.SetMovementEnabled(true);

    }
    public void Exit()
    {
        Debug.Log("You left the state: AGILE IDLE");
    }

    public void Update()
    {
        /*Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));       
        if (input.magnitude > 0.01f)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.moveState); //Pasaje a estado de movimiento
        }*/
    }
}
