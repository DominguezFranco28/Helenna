using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildClimbState : IState
    
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _climbDetector;
    private Collider2D _ignoredClimbable;
    private Vector2 _climbDirection;

    public ChildClimbState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector climbDetector)
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
            float verticalInput = _childPlayerBehaviour.LastCardinalInput.y; //capturo el input vertical
            if (verticalInput > 0)
            {
                _climbDirection = Vector2.up; 
            }
            else if (verticalInput < 0)
            {
                _climbDirection = Vector2.down; 
            }
            else
            {
                //direcion por defecto hacia aarriba por si algun vegano por X motivos se queda quieto sin inputs y bugea el trigger, lo saca  oibligado de la escalera asi.
                _climbDirection = Vector2.up;
            }
            _childPlayerBehaviour.SetMovementInput(_climbDirection);
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
          //  _childPlayerBehaviour.transform.position += Vector3.up * 0.15f;
        }
        _childPlayerBehaviour.Animator.SetBool("isClimbing", false);
        _childPlayerBehaviour.PlayerCollider.isTrigger = false;
        //_childPlayerBehaviour.StopMovement();
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
        SFXManager.Instance.StopLoop();
    }


    public void Update()
    {


        if (!_climbDetector.CanClimb)
        {
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }
    }
}
