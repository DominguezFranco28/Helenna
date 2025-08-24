using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;

public class ImpulseState :  IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private float _timer = 0f;
    private float _waitDuration = 0.4f;
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
        _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
        
        //Logica similar al estaod de Jump del perro, pero ahora necesito guardar el ultimo input nomas, no pos de la plataforma.
        Vector2 lastInput = _oldPlayerBehaviour.LastMovementInput;
        _oldPlayerBehaviour.Animator.SetFloat("Horizontal", lastInput.x);
        _oldPlayerBehaviour.Animator.SetFloat("Vertical", lastInput.y);
        _oldPlayerBehaviour.Animator.SetFloat("Speed", lastInput.magnitude);
        _oldPlayerBehaviour.SetMovementEnabled(false);
        _timer = 0f;
        _timerStarted = false; //mantenemos el timer apagado, quiero que se prenda solo con los imputs

        Debug.Log(lastInput);


    }

    public void Exit()
    {
        Debug.Log("You left the state: IMPULSE");
        _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", false);
        //me daba problemas al no resetear nunca el released del brazo

    }

    public void Update()
    {
        if (!_timerStarted)
        {
            if (Input.GetMouseButtonDown(0)) //Left click = push
            {
                _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");
                _oldPlayerBehaviour.ArmRelease = true; // Set the flag to true to prevent multiple arm releases
                _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Push);
                _timerStarted = true;
            }

            if (Input.GetMouseButtonDown(1)) // 
            {
                if (_oldPlayerBehaviour.ArmRelease)
                {
                    _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");// Set the flag to true to prevent multiple arm releases
                    _oldPlayerBehaviour.ArmPulled = true; //si uso el click derecho y el brazo fue previamente liberado, acciono el tiron del brazo
                                                          //desde el script del armbullet, se gestiona la atraccion si esta bandera del behaviour esta activada.
                    _timerStarted = true;
                }
                _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");// Set the flag to true to prevent multiple arm releases
                _oldPlayerBehaviour.ArmRelease = true; // Set the flag to true to prevent multiple arm releases
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
