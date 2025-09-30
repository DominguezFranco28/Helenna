using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMoveState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _climbDetector;
    private bool subbed = false;
    public ChildMoveState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector climbDetector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._climbDetector = climbDetector;
    }

    private void OnMove(Vector2 movement)
    {
        _childPlayerBehaviour.SetMovementInput(movement);

        if (movement.magnitude <= 0.01f && !_climbDetector.CanClimb)
        {
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }

    }
    private void OnAction()
    {
        if (_climbDetector.CanUseZipline)
        {
            _childStateMachine.TransitionTo(_childStateMachine.ziplineState);
        }
    }

    public void Enter()
    {
        Debug.Log("You entered the state: CHILD MOVE");

        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;

                InputManager.Instance.Move += OnMove;
                InputManager.Instance.ActionPressed += OnAction;
            }
        }

        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
        SFXManager.Instance.PlayLoop(_childPlayerBehaviour.StepsSFX);
    }

    public void Exit()
    {
        Debug.Log("You left the state: CHILD MOVE");
        SFXManager.Instance.StopLoop();

        if (subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = false;

                InputManager.Instance.Move -= OnMove;
            }
        }


    }

    public void Update()
    {

    }
}
