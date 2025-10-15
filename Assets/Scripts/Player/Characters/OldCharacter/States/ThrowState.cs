using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class ThrowState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private AgilePlayerController _rexController;

    private float _throwDelay = 1f;
    private float _throwTimer;
    private bool _delayCompleted;
    private bool subbed = false;
    public ThrowState(OldPlayerBehaviour oldPlayer, OldStateMachine oldStateMachine, AgilePlayerController rex)
    {
        this._oldPlayerBehaviour = oldPlayer;
        this._oldStateMachine = oldStateMachine;
        _rexController = rex;
    }


    public void Enter()
    {
        Debug.Log("You entered the state:  GRAB");
        Vector2 throwDir = _oldPlayerBehaviour.LastMovementInput;
        _oldPlayerBehaviour.StopMovement();
        _oldPlayerBehaviour.SetMovementEnabled(false);
        _oldPlayerBehaviour.Animator.SetBool("IsSliding", true);
        SFXManager.Instance.PlaySFX(_oldPlayerBehaviour.GrabSFX);
        Debug.Log(throwDir);
        _throwTimer = 0f;
        _delayCompleted = false;

        _rexController.ThrowDirection(throwDir);


        //_oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
        //_oldPlayerBehaviour.Animator.SetFloat("Horizontal", lastInput.x);
        //_oldPlayerBehaviour.Animator.SetFloat("Vertical", lastInput.y);
        //_oldPlayerBehaviour.Animator.SetFloat("Speed", lastInput.magnitude);
    }

    public void Exit()
    {
        _oldPlayerBehaviour.Animator.SetBool("IsSliding", false);
        SFXManager.Instance.PlaySFX(_oldPlayerBehaviour.ThrowSFX);
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

        //a cada uno le puse su timer porque quiero que als dos maquinas se disparen a la vez asi cad auno ejecuta sus animaciones y sonidos
    }     
}
