using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;

public class ImpulseState : IState
{
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private OldStateMachine _oldStateMachine;
    private AnchorDetector _anchorDetector;
    public ImpulseState(OldPlayerBehaviour oldPlayer, OldStateMachine oldStateMachine, AnchorDetector anchorDetector)
    {
        _oldPlayerBehaviour = oldPlayer;
        _oldStateMachine = oldStateMachine;
        _anchorDetector = anchorDetector;
    }
    public void Enter()
    {
        Debug.Log("You entered the state: IMPULSE");

        Vector2 lastInput = _oldPlayerBehaviour.LastMovementInput;

        _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
        _oldPlayerBehaviour.Animator.SetFloat("Horizontal", lastInput.x);
        _oldPlayerBehaviour.Animator.SetFloat("Vertical", lastInput.y);
        _oldPlayerBehaviour.Animator.SetFloat("Speed", lastInput.magnitude);

        if (_anchorDetector.ClosestAnchor != null)
        {
            switch (_anchorDetector.CurrentAnchor)
            {
                case AnchorType.Zypline:
                    Debug.Log("Cambio a zypline ");
                    _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
                    _oldStateMachine.TransitionTo(_oldStateMachine.ziplineState);

                    break;
                case AnchorType.HookPipe:
                    Debug.Log("Cambio a PIPE ");
                    _oldStateMachine.TransitionTo(_oldStateMachine.hookPipeState);
                    break;
                case AnchorType.HookPoint:
                    Debug.Log("Cambio a hookpoint ");
                    // lógica para HookPoint
                    _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");
                    _oldPlayerBehaviour.ArmRelease = true;
                    _oldPlayerBehaviour.ArmPulled = true;
                    _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Pull); break;
                default:
                    // lógica por defecto o para None
                    break;
            }
            Debug.Log("LOGICA DE DEZPLAZAMIENTO EJECUTADA");
            return;
        }
        //Si no esta focuseando ningun anclaje, que solo dispare el brazo.

        //Tengo que poder switchear el enum entre push y pull con un unico input y mejorar esta logica 
        else
        {
            _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");
            _oldPlayerBehaviour.ArmRelease = true;
            _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Push);
        }
    }
    public void Exit()
    {
        Debug.Log("You left the state: IMPULSE");
        _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", false);
        _oldPlayerBehaviour.StopMovement();
    }

    public void Update()
    {
        if (!_oldPlayerBehaviour.ArmRelease) //El ArmReleased va a estar condicionado por el lifetime de la bala del brazo (ArmBullet.cs)
        {
            _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
    }
}