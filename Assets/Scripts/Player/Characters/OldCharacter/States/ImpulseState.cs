using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;

    public ImpulseState(OldPlayerBehaviour oldPlayer, OldStateMachine oldStateMachine)
    {
        _oldPlayerBehaviour = oldPlayer;
        _oldStateMachine = oldStateMachine;
    }
    public void Enter()
    {
        Debug.Log("You entered the state: IMPULSE");
        _oldPlayerBehaviour.StopMovement();
        _oldPlayerBehaviour.Animator.SetTrigger("IsImpulsing"); 
    }

    public void Exit()
    {
        Debug.Log("You left the state: IMPULSE");
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Left click = push
        {
            _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Push);

            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }

        if (Input.GetMouseButtonDown(1)) // Right click = pull
        {
            _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Pull);
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
    }
  
}
