using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class AgilePlayerController : MonoBehaviour
{
    [SerializeField] private AgilePlayerBehaviour _agileBehaviour;
    [SerializeField]private GrabObject _grabObject;
    private AgileStateMachine _agileStateMachine;
    private Vector2 throwdirection = Vector2.zero;
    private bool isThrowed= false;
    private bool _isBeingThrown = false;
    public bool IsBeingThrown { get { return _isBeingThrown; } set { value = _isBeingThrown; }}
    //expongo la instancia de la maquina de estados para poder acceder a ella desde el detector de colisiones (detectar si esta en estado throw)
    public AgileStateMachine StateMachine { get { return _agileStateMachine; } }
    private bool interacting = false;
    private void OnEnable()
    {
        if (InputManager.Instance != null && _agileBehaviour.IsInControll)
        {
            InputManager.Instance.InteractPressed += InteractPressed;
        }
            
    }
    private void OnDisable()
    {
        if (InputManager.Instance != null && _agileBehaviour.IsInControll)
        {
            InputManager.Instance.InteractPressed -= InteractPressed;
        }
            
    }
    private void InteractPressed()
    {
        if (!_agileBehaviour.IsInControll) return;
        interacting = true;
        if (InputManager.Instance != null && _agileBehaviour.IsInControll)
            InputManager.Instance.InvokeAction(() => interacting = false, 0.1f);
    }

    private void Start()
    {
        _agileStateMachine = new AgileStateMachine(_agileBehaviour, _grabObject, this);
        _agileStateMachine.Initialize(_agileStateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_agileBehaviour.IsInControll || _isBeingThrown)
        {
            _agileStateMachine?.Update();
        }

    }

    private void FixedUpdate() //Fue necesario poner el fixedUpdate aca por el salto. Necesito que use el fixedupdate para que no de problema con colisiones, y 
                               // como no hereda de monobehaviour lo tengo que agregar como una interfaz. Desde este metodo, se detecta si el perro esta en un estado que aplique esa interfaz, 
                               //y si lo esta, llama al metodo fixedUpdate, no la update como en el caso normal de el resto de estados. Tener presente para futuras aplicaciones de fisica
    {

        if (_agileStateMachine.CurrentState is IFixedUpdate fixedState
         && (_isBeingThrown))
        {
            fixedState.FixedUpdate();
        }
    }

    public void ThrowDirection(Vector2 throwDir)
    {
        _isBeingThrown = true;
        _agileBehaviour.PendingThrowDirection = throwDir; //le paso la direccion al behaviour de rex
        _agileStateMachine.TransitionTo(_agileStateMachine.thrownState, true);
         //el true para forzar la transicion por mas que rex no este activa su maquina de estados.

        Debug.Log("Throw direction set to: " + _agileBehaviour.PendingThrowDirection);
    }
    public void FinishThrow()
    {
        // llamado desde AgileThrownState 
        _isBeingThrown = false;
        Debug.Log("Finished being thrown.");
        _agileStateMachine.TransitionTo(_agileStateMachine.idleState,true);
    }
}

