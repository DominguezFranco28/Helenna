using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private TriggerDetector _oldActionDetector;
    private ActionLever lever;
    private float _actionDelay = 0.5f;
    private float _actionTimer;
    private bool _delayCompleted;

    public ActionState(OldPlayerBehaviour playerBehaviour, OldStateMachine stateMachine, TriggerDetector detector)
    {
        this._oldPlayerBehaviour = playerBehaviour;
        this._oldStateMachine = stateMachine;
        this._oldActionDetector = detector;
    }

    public void Enter()
    {

        Debug.Log("Accionaste una palanca");
        _oldPlayerBehaviour.Animator.SetTrigger("IsOnAction");
        //_oldPlayerBehaviour.Animator.SetBool("IsHolding", true);
        lever = _oldActionDetector.LevelCollider.GetComponent<ActionLever>();
        _actionTimer = 0f;
        _delayCompleted = false;
        _oldPlayerBehaviour.StopMovement();
        _oldPlayerBehaviour.SetMovementEnabled(false);
        ActivateLever();
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado : ACTION");
      //  _oldPlayerBehaviour.Animator.SetBool("IsOnAction", false);
        _oldActionDetector.CanActivate = true;
    }
    private void ActivateLever()
    {
        if (_oldActionDetector.LevelCollider)
        {
            lever.Activate();
          _oldActionDetector.CanActivate = false;
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
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
            }
        }


    }
}
