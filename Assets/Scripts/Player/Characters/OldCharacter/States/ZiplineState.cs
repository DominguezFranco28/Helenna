using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private AnchorDetector _anchorDetector;
    private OldStateMachine _oldStateMachine;

    private bool subbed = false;

    public ZiplineState(OldPlayerBehaviour player,OldStateMachine oldStateMachine, AnchorDetector anchorDetector)
    {
        _oldPlayerBehaviour = player;
        _anchorDetector = anchorDetector;
        _oldStateMachine = oldStateMachine;
    }

    private void OnSpecialAction()
    {
        _oldPlayerBehaviour.LastMovementInput = _oldPlayerBehaviour.MovementInput;
        _oldStateMachine.TransitionTo(_oldStateMachine.impulseState);
    }

    private void OnMove(Vector2 movement)
    {
        _oldPlayerBehaviour.SetMovementInput(movement);
        if (movement.magnitude <= 0.01f)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
    }

    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;

                InputManager.Instance.SpecialActionPressed += OnSpecialAction;
                InputManager.Instance.Move += OnMove;
            }
        }
        Debug.Log("You entered the state: ZIPLINE");
    }

    public void Exit()
    {
        Debug.Log("You left the state: ZIPLINE");
    }

    public void Update()
    {
        
    }
}
