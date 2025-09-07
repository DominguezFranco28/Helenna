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
        _oldPlayerBehaviour.PerformArmToAnchor(_anchorDetector.ClosestAnchor, true);
        _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
    }
    public void Enter()
    {
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;

                InputManager.Instance.SpecialActionPressed += OnSpecialAction;

            }
        }

        Vector2 lastInput = _oldPlayerBehaviour.LastMovementInput;
        _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
        _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");
        _oldPlayerBehaviour.Animator.SetFloat("Horizontal", lastInput.x);
        _oldPlayerBehaviour.Animator.SetFloat("Vertical", lastInput.y);
        _oldPlayerBehaviour.Animator.SetFloat("Speed", lastInput.magnitude);
        _oldPlayerBehaviour.SetMovementEnabled(false);
        _oldPlayerBehaviour.PerformArmToAnchor(_anchorDetector.ClosestAnchor, false);
        Debug.Log("You entered the state: ZIPLINE");
    }

    public void Exit()
    {
            if (InputManager.Instance != null)
            {
                subbed = false;
                InputManager.Instance.SpecialActionPressed -= OnSpecialAction;
            }
        _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", false); 
        _oldPlayerBehaviour.SetMovementEnabled(true);
        Debug.Log("You left the state: ZIPLINE");
    }

    public void Update()
    {
        _oldPlayerBehaviour.SetMovementEnabled(false);
        //sin setear esto aca, harold podia moverse si quedaba clavado con tirolesa, cambiabas de pj, y molvias a moverte.
    }
}
