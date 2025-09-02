using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPipe : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private AnchorDetector _anchorDetector;
    private OldStateMachine _oldStateMachine;

    private bool subbed = false;

    public HookPipe(OldPlayerBehaviour player, OldStateMachine oldStateMachine, AnchorDetector anchorDetector)
    {
        _oldPlayerBehaviour = player;
        _anchorDetector = anchorDetector;
        _oldStateMachine = oldStateMachine;
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

                InputManager.Instance.Move += OnMove;
            }
        }
        Debug.Log("You entered the state: HOOKPIPE");
    }

    public void Exit()
    {
        Debug.Log("You left the state: HOOKPIPE");
    }

    public void Update()
    {
        
    }
}
