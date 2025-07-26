using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;

public class ImpulseState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private float _timer = 0f;
    private float _waitDuration = 1f;
    private bool _timerStarted = false;
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
        _oldPlayerBehaviour.SetMovementEnabled(false); //tuve que llamar este metodo tambien desde la corutina par ael anclaje y cuando dejab de 
        _timer = 0f;
        _timerStarted = false; //mantenemos el timer apagado, quiero que se prenda solo con los imputs
    }

    public void Exit()
    {
        Debug.Log("You left the state: IMPULSE");
    }

    public void Update()
    {
        if (!_timerStarted)
        {
            if (Input.GetMouseButtonDown(0)) //Left click = push
            {
                _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Push);
                _timerStarted = true;

            }

            if (Input.GetMouseButtonDown(1)) // Right click = pull
            {
                _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Pull);
                _timerStarted = true;

            }
        }
        else
        {
            _timer += Time.deltaTime; //gestiono el salto de estado dsps del timer par aque el pj no se pueda mover mientras vuela el brazo

            if (_timer >= _waitDuration)
            {
                _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
            }
        }
    }    

}
