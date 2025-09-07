using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChildZiplineState : IState
    
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _ziplineDetector;

    private ArmLineController _activeZipline;
    private float _zipProgress = 0f; // progreso 0-1 a lo largo de la zipline, se mide con un lerp y parametros tomados del characterManager (tirolesa viva)

    public ChildZiplineState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector ziplineDetector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._ziplineDetector = ziplineDetector;
    }


    public void Enter()
    {
        Debug.Log("You entered the state:  CHILD ZIPLINE");
        _childPlayerBehaviour.SetMovementEnabled(false);

        _activeZipline = CharacterManager.Instance.GetActiveZipline(); // obtengo la referencia guardada en el manager de la zipline viva
        _childPlayerBehaviour.transform.position = _activeZipline.StartPoint;
        _zipProgress = 0f;

        if (_ziplineDetector.CanUseZipline)
        {
            _childPlayerBehaviour.PlayerCollider.isTrigger = true;
            _childPlayerBehaviour.Animator.SetBool("isClimbing", true);
            SFXManager.Instance.PlayLoop(_childPlayerBehaviour.ClimbSFX);
        }
    }

    public void Exit()
    {
        Debug.Log("You left the state: CHILD ZIPLINE");


        // desactivo colisiones?
        _childPlayerBehaviour.PlayerCollider.isTrigger = false;
        _childPlayerBehaviour.Animator.SetBool("isClimbing", false);
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
        SFXManager.Instance.StopLoop();

        _ziplineDetector.ResetZipline();
        _activeZipline = null;
    }


    public void Update()
    {
        if (_activeZipline == null) return;

        Vector3 startZip = _activeZipline.StartPoint;
        Vector3 endZip = _activeZipline.EndPoint;

        // avanza el progreso de la zipline segun la velocidad de 
        float distance = Vector3.Distance(startZip, endZip);
        _zipProgress += (_childPlayerBehaviour.ZiplineSpeed * Time.deltaTime) / distance;
        _zipProgress = Mathf.Clamp01(_zipProgress);

        // lerp para pegar la posicion a lo largo de la zipline
        _childPlayerBehaviour.transform.position = Vector3.Lerp(startZip, endZip, _zipProgress);

        // cuando llega al final de la linea, baja al suelo y cambia a estado de movimiento
        if (_zipProgress >= 1f)
        {
            _childPlayerBehaviour.transform.position += Vector3.down * 2f; //modificable si es necesario hacer una animacion de bajada
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }
    }
}
