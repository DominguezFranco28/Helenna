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

            _throwTimer += Time.deltaTime;

            // si no termino el timer de reposicion, seguimos mov a Rex
            if (_positionTimer < _positionDuration)
            {
                _positionTimer+= Time.deltaTime;

                // t' se basa ahora en la nueva duracion de reposicion
                float t = Mathf.Clamp01(_positionTimer / _positionDuration); //  Clamp01 para asegurar que t nunca sea > 1

                // calcular pa pos base del hombro en cada frame
                Vector3 haroldWorldPosition = _oldPlayerBehaviour.transform.position;
                Vector3 armOffset = _oldPlayerBehaviour.GetArmOffset();
                Vector3 fixedYOffset = new Vector3(armOffset.x, 0.9f, 0); //
                Vector3 currentTargetPosition = haroldWorldPosition + fixedYOffset;
                //  movimiento suave
                _rexController.transform.position = Vector3.Lerp(_startRexPosition, currentTargetPosition, t);
            }
            else
            {

                Vector3 haroldWorldPosition = _oldPlayerBehaviour.transform.position;
                Vector3 armOffset = _oldPlayerBehaviour.GetArmOffset();
                Vector3 fixedYOffset = new Vector3(armOffset.x, 0.9f, 0); 

                _rexController.transform.position = haroldWorldPosition + fixedYOffset;
            }

            if (!_delayCompleted && _throwTimer >= _throwDelay)
            {
                _delayCompleted = true;
                _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
            }
        }
    }
}
