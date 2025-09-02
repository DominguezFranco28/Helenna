using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private AnchorDetector _anchorDetector;
    private OldStateMachine _oldStateMachine;

    public ZiplineState(OldPlayerBehaviour player,OldStateMachine oldStateMachine, AnchorDetector anchorDetector)
    {
        _oldPlayerBehaviour = player;
        _anchorDetector = anchorDetector;
        _oldStateMachine = oldStateMachine;
    }

    public void Enter()
    {
       Debug.Log("You entered the state: ZIPLINE");
    }

    public void Exit()
    {
        Debug.Log("You left the state: ZIPLINE");
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
            _oldPlayerBehaviour.LastMovementInput = _oldPlayerBehaviour.MovementInput;
            _oldStateMachine.TransitionTo(_oldStateMachine.impulseState);
        }
    }
}
