using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildClimbState : IState
    
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ClimbDetector _climbDetector;
    private Collider2D _ignoredClimbable;

    public ChildClimbState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ClimbDetector climbDetector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._climbDetector = climbDetector;
    }
    public void Enter()
    {
        Debug.Log("You entered the state:  CHILD CLIMB");
        if (_climbDetector.Climbable != null)
        {
            _childPlayerBehaviour.PlayerCollider.enabled = false;
            _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.ClimbSpeed);
            _childPlayerBehaviour.Animator.SetBool("isClimbing", true);
            SFXManager.Instance.PlayLoop(_childPlayerBehaviour.ClimbSFX);
        }
    }

    public void Exit()
    {
        Debug.Log("You left the state: CHILD CLIMB");
        // Restore colissions 
        if (_ignoredClimbable != null)
        {
            //move the player up a bit to give the sensation of jumping after climbing
            _childPlayerBehaviour.transform.position += Vector3.up * 0.15f;
        }
        _childPlayerBehaviour.Animator.SetBool("isClimbing", false);
        _childPlayerBehaviour.PlayerCollider.enabled = true;
        _childPlayerBehaviour.StopMovement();
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
        SFXManager.Instance.StopLoop();
    }


    public void Update()
    {

        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 climbVelocity = new Vector2(0f, vertical * _childPlayerBehaviour.ClimbSpeed);
        _childPlayerBehaviour.SetMovementInput(climbVelocity);
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.ClimbSpeed);

        if (!_climbDetector.CanClimb)
        {
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }
    }
}
