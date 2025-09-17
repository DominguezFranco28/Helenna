using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Windows;

public class GrabState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private GrabObject grabObject;
    private AgilePlayerController _rexController;

    private float _throwDelay = 5f;
    private float _throwTimer;
    private bool _delayCompleted;

    public GrabState(OldPlayerBehaviour oldPlayer, OldStateMachine oldStateMachine, GrabObject grabObject, AgilePlayerController rex)
    {
        this._oldPlayerBehaviour = oldPlayer;
        this._oldStateMachine = oldStateMachine;
        this.grabObject = grabObject;
        _rexController = rex;
    }



    public void Enter()
    {
        Debug.Log("You entered the state:  GRAB");
        _throwTimer = 0f;
        _delayCompleted = false;
        _oldPlayerBehaviour.StopMovement();
        Vector2 throwDir = _oldPlayerBehaviour.LastMovementInput;

        _rexController.ThrowDirection(throwDir);


        //_oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
        //_oldPlayerBehaviour.Animator.SetFloat("Horizontal", lastInput.x);
        //_oldPlayerBehaviour.Animator.SetFloat("Vertical", lastInput.y);
        //_oldPlayerBehaviour.Animator.SetFloat("Speed", lastInput.magnitude);
    }

    public void Exit()
    {
        Debug.Log("You exited the state:  GRAB");
    }

    public void Update()
    {
        // Wait for delay
        if (!_delayCompleted)
        {
            _throwTimer += Time.deltaTime;
            if (_throwTimer >= _throwDelay)
            {
                _delayCompleted = true;
                Debug.Log("End of delay");

            }
            return; // skip the update until delay is over
        }
        _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
    }     
}
