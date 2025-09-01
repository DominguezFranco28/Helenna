using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileMoveState : IState
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;

    private bool doingAction = false;

    private bool subbed = false;

    private void ActionPressed()
    {
        doingAction = true;
        if (InputManager.Instance != null)
            InputManager.Instance.InvokeAction(() => doingAction = false, 0.1f);
    }
    private void OnMove(Vector2 movement)
    {
        _agilePlayerBehaviour.SetMovementInput(movement);
        if (movement.magnitude <= 0.1f)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
            return; //return para evitar que siga evaluando el resto de condiciones.
        }
    }

    public AgileMoveState(AgilePlayerBehaviour agilePlayerBehaviour, AgileStateMachine agileStateMachine)
    {
        this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;

    }
    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.ActionPressed += ActionPressed;
                InputManager.Instance.Move += OnMove;
            }
        }
        
        Debug.Log("You entered the state: AGILE MOVE");
        SFXManager.Instance.PlayLoop(_agilePlayerBehaviour.StepsSFX);
    }

    public void Exit()
    {
        Debug.Log("You left the state: AGILE MOVE");
        SFXManager.Instance.StopLoop();
    }
    
    public void Update()
    {
        /*Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _agilePlayerBehaviour.SetMovementInput(input);
        if (input.magnitude <= 0.1f)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
            return; //return para evitar que siga evaluando el resto de condiciones.
        }*/
        if (_agilePlayerBehaviour.CanDig == true)
        {
            _agileStateMachine.TransitionTo(_agileStateMachine.digState); 
            return;
        }
        if (_agilePlayerBehaviour.CanJump && _agilePlayerBehaviour.DelayCompleted && doingAction)
        {

            _agilePlayerBehaviour.StopMovement();
            _agileStateMachine.TransitionTo(_agileStateMachine.jumpState);
            return;
        }

    }
}
