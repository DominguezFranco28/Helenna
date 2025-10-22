using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class ThrowState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private AgilePlayerController _rexController;
    private Vector3 _armWorldPosition;
    private Vector3 _startRexPosition;
    private float _positionDuration = 0.4f; //estos timer aparte para la subida de rex al punto de lanzamiento, quiero mantenerlo separado de cuanto tarda en lanzarlo
    private float _positionTimer;
    private float _throwDelay = 1f;
    private float _throwTimer;
    private bool _delayCompleted;
    private bool subbed = false;
    public ThrowState(OldPlayerBehaviour oldPlayer, OldStateMachine oldStateMachine, AgilePlayerController rex)
    {
        this._oldPlayerBehaviour = oldPlayer;
        this._oldStateMachine = oldStateMachine;
        _rexController = rex;
    }


    public void Enter()
    {
        Debug.Log("You entered the state:  Throw-GRAB");

        Vector2 throwDir = _oldPlayerBehaviour.LastMovementInput;
        _oldPlayerBehaviour.StopMovement();
        _oldPlayerBehaviour.SetMovementEnabled(false);
        _oldPlayerBehaviour.Animator.SetBool("IsGrabbing", true);
        SFXManager.Instance.PlaySFX(_oldPlayerBehaviour.GrabSFX);
        _positionTimer = 0f;
        _throwTimer = 0f;
        _delayCompleted = false;

        //finalposrex necesario porque sino seteaba al perro en la una pos mundia determinada, no relativa al hombro de harold
        Vector3 finalPosRex = _oldPlayerBehaviour.transform.position + _oldPlayerBehaviour.GetArmOffset();
        _armWorldPosition = finalPosRex + new Vector3(0, 0.9f, 0);
        _startRexPosition = _rexController.transform.position; //guardo pos inciial de rex en donde comienza la subida
        _rexController.ThrowDirection(throwDir);


        //_oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
        //_oldPlayerBehaviour.Animator.SetFloat("Horizontal", lastInput.x);
        //_oldPlayerBehaviour.Animator.SetFloat("Vertical", lastInput.y);
        //_oldPlayerBehaviour.Animator.SetFloat("Speed", lastInput.magnitude);
    }

    public void Exit()
    {
        _oldPlayerBehaviour.Animator.SetBool("IsGrabbing", false);
        SFXManager.Instance.PlaySFX(_oldPlayerBehaviour.ThrowSFX);
        Debug.Log("You exited the state:  GRAB");
    }

    public void Update()
    {
        {
            // Incrementamos ambos timers
            _throwTimer += Time.deltaTime;

            // Si el tiempo de subida no ha terminado, movemos a Rex
            if (_positionTimer < _positionDuration)
            {
                _positionTimer+= Time.deltaTime;

                // El factor 't' se basa ahora en la nueva duración corta
                float t = Mathf.Clamp01(_positionTimer / _positionDuration); // Usamos Clamp01 para asegurar que t nunca sea > 1

                // 1. Calcular la posición base (hombro) de Harold en este frame
                Vector3 haroldWorldPosition = _oldPlayerBehaviour.transform.position;
                Vector3 armOffset = _oldPlayerBehaviour.GetArmOffset();
                Vector3 fixedYOffset = new Vector3(armOffset.x, 0.9f, 0); //
                Vector3 currentTargetPosition = haroldWorldPosition + fixedYOffset;
                // 3. Interpolación (movimiento suave)
                _rexController.transform.position = Vector3.Lerp(_startRexPosition, currentTargetPosition, t);
            }
            else
            {
                // Una vez terminada la subida, Rex se queda pegado al hombro,
                // siguiendo a Harold si se mueve (sin la interpolación Lerp)

                // 1. Obtener la posición mundial de Harold
                Vector3 haroldWorldPosition = _oldPlayerBehaviour.transform.position;

                // 2. Obtener el offset del brazo de Harold (que tiene el X correcto)
                Vector3 armOffset = _oldPlayerBehaviour.GetArmOffset();

                // 3. Forzamos la Y a 1.6f (o el valor que decidas)
                Vector3 fixedYOffset = new Vector3(armOffset.x, 0.9f, 0); 

                _rexController.transform.position = haroldWorldPosition + fixedYOffset;
            }

            // Comprobación para la transición de estado (usa el throwDelay largo)
            if (!_delayCompleted && _throwTimer >= _throwDelay)
            {
                _delayCompleted = true;
                // Ya no necesitamos asignar _throwTimer = _throwDelay, solo transicionamos
                Debug.Log("End of delay, transitioning to Idle.");

                // Al terminar el estado, el lanzamiento ocurre en la lógica de transición
                _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
            }

            // Si la subida terminó, pero el throwDelay no, el Update sigue funcionando 
            // y mantiene a Rex pegado al hombro (código del else de arriba).
        }
    }
}
