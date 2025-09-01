using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;

public class ImpulseState :  IState
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
        _oldPlayerBehaviour.StopMovement();
        _oldPlayerBehaviour.Animator.SetBool("IsImpulsing", true);
        
        //Logica similar al estaod de Jump del perro, pero ahora necesito guardar el ultimo input nomas, no pos de la plataforma.
        Vector2 lastInput = _oldPlayerBehaviour.LastMovementInput;
        _oldPlayerBehaviour.Animator.SetFloat("Horizontal", lastInput.x);
        _oldPlayerBehaviour.Animator.SetFloat("Vertical", lastInput.y);
        _oldPlayerBehaviour.Animator.SetFloat("Speed", lastInput.magnitude);
        _oldPlayerBehaviour.SetMovementEnabled(false);


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
        if (!_oldPlayerBehaviour.CanMove)  //El CanMove del player va a estar condicionado por el lifetime de la bala del brazo (ArmBullet.cs)

        {
            if (_anchorDetector.ClosestAnchor != null)
               {
                 Debug.Log("LOGICA DE DEZPLAZAMIENTO EJECUTADA");
               
               //return; //si hay un punto de anclaje cercano, no hago nada dsps, espero a que el jugador se desplace o lance el brazo
            }
            if (_oldPlayerBehaviour.ArmRelease)   //desde el script del armbullet, se gestiona la atraccion si esta bandera del behaviour esta activada.
              {//HOOKS CON APUNTADO, Atraccion a puntos de anclaje
                //esta condicion puede llegar a borrarse si pasamos a depender siempre del lock on, de momento lo dejo a modo de Hooks que necesiten Aim
                 _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");// Set the flag to true to prevent multiple arm releases
                 _oldPlayerBehaviour.ArmPulled = true; //si  el brazo fue previamente liberado, acciono el tiron del brazo
              }
           else //solo arrojar el brazo 
             {
               _oldPlayerBehaviour.Animator.SetTrigger("ReleaseArm");
               _oldPlayerBehaviour.ArmRelease = true; // 
               _oldPlayerBehaviour.PerformThrowArm(ImpulseType.Push);    
             }  
               
            
            //recordar implementar nueva logica para la atraccion del brazo. Saltos a diferentes states segun el tipo de anclaje
            //
            //Tengo que poder switchear el enum entre push y pull con un unico input
        }
        else
        {
                _oldStateMachine.TransitionTo(_oldStateMachine.idleState);
        }
    }

}
