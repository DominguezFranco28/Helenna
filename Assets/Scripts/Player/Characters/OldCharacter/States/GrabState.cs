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
    private AgilePlayerController _rexController;

    private float _throwDelay = 2f;
    private float _throwTimer;
    private bool _delayCompleted;
    private bool subbed = false;
    public GrabState(OldPlayerBehaviour oldPlayer, OldStateMachine oldStateMachine, AgilePlayerController rex)
    {
        this._oldPlayerBehaviour = oldPlayer;
        this._oldStateMachine = oldStateMachine;
        _rexController = rex;
    }

    private void OnMove(Vector2 movement)
    {
        _oldPlayerBehaviour.SetMovementInput(new Vector2(0, 0));
    }

    public void Enter()
    {
        Debug.Log("You entered the state:  GRAB");
        Vector2 throwDir = _oldPlayerBehaviour.LastMovementInput;
        _oldPlayerBehaviour.StopMovement();
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = false;
                InputManager.Instance.Move += OnMove;
            }
        }
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
        Debug.Log("You exited the state:  GRAB");
        if (subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.Move -= OnMove;
            }
        }
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
