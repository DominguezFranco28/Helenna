using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildIdleState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    public ChildIdleState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
    }

    public void Enter()
    {
        Debug.Log("You entered the state: CHILD IDLE");
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

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
    }
}
   
