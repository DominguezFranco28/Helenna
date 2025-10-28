using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildActionState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _actionDetector;
    private Collider2D _collider2D;
    private ActionLever lever;
    private float _actionDelay = 0.5f;
    private float _actionTimer;
    private bool _delayCompleted;

    public ChildActionState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector detector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._actionDetector = detector;
    }

    public void Enter()
    {

        Debug.Log("Accionaste una palanca");
        lever = _actionDetector.LeverCollider.GetComponent<ActionLever>();
        _childPlayerBehaviour.Animator.SetTrigger("IsOnAction");
            _actionTimer = 0f;
            _delayCompleted = false;
            _childPlayerBehaviour.StopMovement();
        _childPlayerBehaviour.SetMovementEnabled(false);
        ActivateLever();
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado : ACTION");
       // _childPlayerBehaviour.Animator.SetBool("IsHolding", false);
        //lever.ResetLever();
        _actionDetector.CanActivate = true;
    }
    private void ActivateLever()
    {
        if (_actionDetector.LeverCollider)
        {
            lever.Activate();
            _actionDetector.CanActivate = false;

        }
    }
    public void Update()
    {
        // Wait for delay >> misma logica del dig del perro, pero le agrego el cd par amarcar el cambio de stado
        if (!_delayCompleted)
        {
            _actionTimer += Time.deltaTime;
            if (_actionTimer >= _actionDelay)
            {
                _delayCompleted = true;
                Debug.Log("End of delay");
                _childStateMachine.TransitionTo(_childStateMachine.idleState);
            }
        }


    }
}
