using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildIdleState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private bool subbed = false;

    public ChildIdleState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
    }

    private void OnMove(Vector2 movement)
    {
        if (Mathf.Abs(movement.x) > 0.1f || Mathf.Abs(movement.y) > 0.1f)
        {
            _childStateMachine.TransitionTo(_childStateMachine.moveState);
            return;
        }

        // Detect if it can climb
        if (_childPlayerBehaviour.ClimbDetector.CanClimb && Mathf.Abs(movement.y) > 0.1f)
        {
            _childStateMachine.TransitionTo(_childStateMachine.climbState);
            return;
        }
    }

    public void Enter()
    {
        Debug.Log("You entered the state: CHILD IDLE");
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.Move += OnMove;
            }
            else
            {
                Debug.LogError("Input manager not found");
            }
        }
        
        _childPlayerBehaviour.StopMovement(); 
        _childPlayerBehaviour.SetMovementEnabled(true);
    }

    public void Exit()
    {
        Debug.Log("You left the state: CHILD IDLE");
    }

    public void Update()
    {
        //Movement behavior slightly different from the rest of the players,
        //since the girl implements a "false verticality" with climbing.
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        /*
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            _childStateMachine.TransitionTo(_childStateMachine.moveState);
            return;
        }

        // Detect if it can climb
        if (_childPlayerBehaviour.ClimbDetector.CanClimb && Mathf.Abs(vertical) > 0.1f)
        {
            _childStateMachine.TransitionTo(_childStateMachine.climbState);
            return;
        }
        */
    }
}
   
