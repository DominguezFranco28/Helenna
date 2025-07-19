using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private JumpDetector _jumpDetector;
    private float _jumpDelay = 0.2f;
    private float _jumpTimer;
    private bool _delayCompleted;
    public JumpState(OldPlayerBehaviour oldPlayer, OldStateMachine oldStateMachine, JumpDetector jumpDetector)
    {
        _oldPlayerBehaviour = oldPlayer;
        _oldStateMachine = oldStateMachine;
        _jumpDetector = jumpDetector;
    }
    public void Enter()
    {
        Debug.Log("You entered the state: JUMP");
        _oldPlayerBehaviour.StopMovement();
        _jumpTimer = 0f;
        _delayCompleted = false;
        _oldPlayerBehaviour.Animator.SetTrigger("IsImpulsing"); //cambiar por animacion correspondiente cuando la tenga
    }

    public void Exit()
    {
        Debug.Log("You left the state: JUMP");
        //if (_holeDetector.CanDig == false) //misma logic aque la animaicon del eprro
        //{
        //    _agilePlayerBehaviour.Animator.SetBool("Dig", false);
        //}
    }

    public void Update()
    {
        if (!_delayCompleted)
        {
            _jumpTimer += Time.deltaTime;
            if (_jumpTimer >= _jumpDelay)
            {
                _delayCompleted = true;
                Debug.Log("End of delay");

            }
            return; // skip the update until delay is over
        }
        Vector2 input = new Vector2(0, -1); //set the horizontal move to 0
        _oldPlayerBehaviour.SetMovementInput(input);
        _oldPlayerBehaviour.Animator.SetTrigger("IsImpulsing");//cambiar por animacion correspondiente cuando la tenga

        if (!_jumpDetector.CanJump) //Left click = push

        {

            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
    }
}
