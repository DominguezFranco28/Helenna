using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMoveState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    public ChildMoveState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entraste al estado : Child Move");
        _childPlayerBehaviour.SetMovementEnabled(true);
        SFXManager.Instance.PlayLoop(_childPlayerBehaviour.StepsSFX);
    }

    public void Exit()
    {
        Debug.Log("saliste del estado: movimiento");
        SFXManager.Instance.StopLoop();
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _childPlayerBehaviour.SetMovementInput(input);
        if (input.magnitude <= 0.01f)
        //_childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
        {
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }
    }
}
