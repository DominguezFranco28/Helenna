using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ChildZiplineState : IState
    
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _ziplineDetector;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private ArmLineController _activeZipline;
    private float _zipProgress = 0f; // progreso 0-1 a lo largo de la zipline, se mide con un lerp y parametros tomados del characterManager (tirolesa viva)
    private bool subbed = false;

    private bool vfxPlayed = false;

    public ChildZiplineState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector ziplineDetector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._ziplineDetector = ziplineDetector;
    }
    private void OnMove(Vector2 movement)
    {

            _childPlayerBehaviour.SetMovementInput(new Vector2(0, movement.y));

       
    }

    public void Enter()
    {
        Debug.Log("You entered the state:  CHILD ZIPLINE");
        if (!subbed)
        {
            if (InputManager.Instance != null)
            {
                subbed = true;
                InputManager.Instance.Move += OnMove;
            }
        }

        _childPlayerBehaviour.SetMovementEnabled(false);

        _activeZipline = CharacterManager.Instance.GetActiveZipline(); // obtengo la referencia guardada en el manager de la zipline viva
        if (_activeZipline == null) return;
        // define puntos de inicio y fin según el detector
        _startPoint = _ziplineDetector.GetEntryPoint(_childPlayerBehaviour.transform.position, _activeZipline);
        _endPoint = _ziplineDetector.GetTargetPoint(_activeZipline);

      // colocamos a nina en el inicio de la tirolesa
        _childPlayerBehaviour.transform.position = _startPoint;
        _zipProgress = 0f;
        if (_ziplineDetector.CanUseZipline)
        {
            _childPlayerBehaviour.PlayerCollider.isTrigger = true;//no se si le vamos a dar uso al final a esto,.. pero por las dudas lo dejo
            _childPlayerBehaviour.Animator.SetBool("IsOnZipline", true);
            SFXManager.Instance.PlayLoop(_childPlayerBehaviour.ZiplineSFX);
            CharacterManager.Instance.IsOnZipline = true;
        }
    }

    public void Exit()
    {
        Debug.Log("You left the state: CHILD ZIPLINE");
        if (InputManager.Instance != null)
        {
            subbed = false;
            InputManager.Instance.Move -= OnMove;
        }

        // desactivo colisiones?
        _childPlayerBehaviour.PlayerCollider.isTrigger = false;
        _childPlayerBehaviour.Animator.SetBool("IsOnZipline", false);
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
        SFXManager.Instance.StopLoop();

        _ziplineDetector.ResetZipline();
        _activeZipline = null;
        CharacterManager.Instance.IsOnZipline = false;
    }


    public void Update()
    {
        if (_activeZipline == null) return;

        // avanza el progreso de la zipline segun la velocidad de 
        float distance = Vector3.Distance(_startPoint, _endPoint);
        _zipProgress += (_childPlayerBehaviour.ZiplineSpeed * Time.deltaTime) / distance;
        _zipProgress = Mathf.Clamp01(_zipProgress);

        // lerp para pegar la posicion a lo largo de la zipline
        _childPlayerBehaviour.transform.position = Vector3.Lerp(_startPoint, _endPoint, _zipProgress);

        if (_zipProgress >= 0.1f) ZiplineVFX();

        // cuando llega al final de la linea, baja al suelo y cambia a estado de movimiento
        if (_zipProgress >= 1f)
        {
            vfxPlayed = false;
          
            _childPlayerBehaviour.transform.position += Vector3.down * 2f; //modificable si es necesario hacer una animacion de bajada
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }
    }

    private void ZiplineVFX()
    {
        if (!vfxPlayed)
        {
            vfxPlayed = true;
            
            ParticleSystem vfx = _childPlayerBehaviour.gameObject.GetComponentInChildren<ParticleSystem>();
            if (vfx)
            {
                vfx.Play();
            }
        }
        
    }
}
