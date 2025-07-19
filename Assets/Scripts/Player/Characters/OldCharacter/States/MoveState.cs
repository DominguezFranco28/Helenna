using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private JumpDetector _jumpDetector;
    public MoveState(OldPlayerBehaviour oldPlayerBehaviour, OldStateMachine oldStateMachine, JumpDetector jumpDetector)
    {
        this._oldPlayerBehaviour = oldPlayerBehaviour;
        this._oldStateMachine = oldStateMachine;
        this._jumpDetector = jumpDetector;
    }

    public void Enter()
    {
        Debug.Log("You entered the state: OLD MOVE");
        SFXManager.Instance.PlayLoop(_oldPlayerBehaviour.StepsSFX);
    }

    public void Exit()
    {
        Debug.Log("You left the state: OLD MOVE");
        SFXManager.Instance.StopLoop();
    }

    public void Update()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
       _oldPlayerBehaviour.SetMovementInput(input);
        if (input.magnitude <= 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.impulseState); 
        }
        if (_jumpDetector.CanJump)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.jumpState);
        }
    }
}
