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
    private bool _isBeingPulled = false;
    public bool IsBeingThrown { get { return _isBeingThrown; } set { value = _isBeingThrown; }}
    public bool IsBeingPulled { get { return _isBeingThrown; } set { value = _isBeingPulled; }}
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
        if (_agileBehaviour.IsInControll || _isBeingThrown || _isBeingPulled) //quiero mantener activa la maquina de estados de rex si interactua con Harold
        {
            _agileStateMachine?.Update();
            /*//HERRAMIENTA DEBUGEO, TP A TODOS LOS PERSONAJES A LA POSICION DEL ACTIVO
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CharacterManager.Instance.TeleportAllToCurrent();
            }*/
        }

    }

    private void FixedUpdate() //Fue necesario poner el fixedUpdate aca por el salto. Necesito que use el fixedupdate para que no de problema con colisiones, y 
                               // como no hereda de monobehaviour lo tengo que agregar como una interfaz. Desde este metodo, se detecta si el perro esta en un estado que aplique esa interfaz, 
                               //y si lo esta, llama al metodo fixedUpdate, no la update como en el caso normal de el resto de estados. Tener presente para futuras aplicaciones de fisica
    {

        if (_agileStateMachine.CurrentState is IFixedUpdate fixedState && (_isBeingThrown || _isBeingPulled))
        {
            fixedState.FixedUpdate();
        }
    }

    public void ThrowDirection(Vector2 throwDir) //llamado desde el estado Throw de harold, para pasarle la direccion en la que rex debe ser lanzado
    {
        if (_isBeingPulled) return;
        _isBeingThrown = true;
        _agileBehaviour.PendingThrowDirection = throwDir; //le paso la direccion al behaviour de rex
        _agileStateMachine.TransitionTo(_agileStateMachine.thrownState, true);
         //el true para forzar la transicion por mas que rex no este activa su maquina de estados.

       // Debug.Log("Throw direction set to: " + _agileBehaviour.PendingThrowDirection);
    }
    public void PullDirection (Vector2 pullDirection) //no sigue la misma logica ecacta que el thrown, porque este metodo se llamada desde la colission del Armbullet no desde estado especifico de Harold
    {
        if (_isBeingThrown) return; // si esta siendo lanzado no puede ser atraido
        _isBeingPulled = true;
        _agileBehaviour.PendingPulledDirection = pullDirection;
        _agileStateMachine.TransitionTo(_agileStateMachine.pulledState, true);
        //Debug.Log("Pull direction set to: " + _agileBehaviour.PendingPulledDirection);
    }
    
    public void FinishThrow()
    {
        // llamado desde AgileThrownState 
        _isBeingThrown = false;
       // Debug.Log("Finished being thrown.");
        _agileStateMachine.TransitionTo(_agileStateMachine.idleState,true);
    }
    public void FinishPull()
    {
        // llamado desde AgilePulledState 
        _isBeingPulled = false;
      //  Debug.Log("Finished being pulled.");
        _agileStateMachine.TransitionTo(_agileStateMachine.idleState, true);
    }
}

