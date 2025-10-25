using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildClimbState : IState
    
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _climbDetector;
    private Collider2D _ignoredClimbable;
    private bool subbed = false;

    public ChildClimbState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector climbDetector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._climbDetector = climbDetector;
    }

    private void OnMove(Vector2 movement)
    {
        if (_climbDetector.CanClimb)
        { // Solo movimiento vertical
            float verticalInput = Mathf.Abs(movement.y) > 0.1f ? movement.y : 0f;
            Vector2 climbVelocity = new Vector2(0f, movement.y);
            _childPlayerBehaviour.SetMovementInput(climbVelocity);
        }
        else
        {

                _childStateMachine.TransitionTo(_childStateMachine.moveState);
            
                
        }
    }

    public void Enter()
    {
        Debug.Log("You entered the state:  CHILD CLIMB");
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.Move += OnMove;
            }
        }
        
        if (_climbDetector.Climbable != null)
        {
            _childPlayerBehaviour.SetMovementInput(Vector2.zero);
            _childPlayerBehaviour.PlayerCollider.isTrigger = true;
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
        _childPlayerBehaviour.PlayerCollider.isTrigger = false;
        _childPlayerBehaviour.StopMovement();
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
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

        if (!_climbDetector.CanClimb)
        {
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }
    }
}
