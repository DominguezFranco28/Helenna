using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        _oldStateMachine.TransitionTo(_oldStateMachine.slideState);
    }
    public void Enter()
    {
        _oldPlayerBehaviour.PerformThrowArmToAnchor(_anchorDetector.ClosestAnchor);
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;

                InputManager.Instance.SpecialActionPressed += OnSpecialAction;

            }
        }


        Debug.Log("You entered the state: ZIPLINE");
    }

    public void Exit()
    {
            if (InputManager.Instance != null)
            {
                subbed = true;

                InputManager.Instance.SpecialActionPressed -= OnSpecialAction;

            }
        
        Debug.Log("You left the state: ZIPLINE");
    }

    public void Update()
    {
        Debug.Log("You are in the state: ZIPLINE");
    }
}
