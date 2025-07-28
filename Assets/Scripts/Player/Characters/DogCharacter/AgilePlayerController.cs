using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgilePlayerController : MonoBehaviour
{
    [SerializeField] private AgilePlayerBehaviour _agileBehaviour;
    [SerializeField]private PlatformDetector _platformDetector;
    [SerializeField]private GrabObject _grabObject;
    private AgileStateMachine _agileStateMachine;
    private void Start()
    {
        _agileStateMachine = new AgileStateMachine(_agileBehaviour, _platformDetector, _grabObject);
        _agileStateMachine.Initialize(_agileStateMachine.idleState);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGamePaused()) return;
        if (_agileBehaviour.isInControll)
        {
            _agileStateMachine?.Update();
            if (_grabObject.PickedObject != null && _grabObject.InColision && Input.GetKeyDown(KeyCode.E))
            {
                _agileStateMachine.TransitionTo(_agileStateMachine.itemState);
            }
        }


    }
    private void FixedUpdate() //Fue necesario poner el fixedUpdate aca por el salto. Necesito que use el fixedupdate para que no de problema con colisiones, y 
       // como no hereda de monobehaviour lo tengo que agregar como una interfaz. Desde este metodo, se detecta si el perro esta en un estado que aplique esa interfaz, 
       //y si lo esta, llama al metodo fixedUpdate, no la update como en el caso normal de el resto de estados. Tener presente para futuras aplicaciones de fisica
    {
        if (_agileBehaviour.isInControll)
        {
            if (_agileStateMachine.CurrentState is IFixedUpdate fixedState)
            {
                fixedState.FixedUpdate();
            }

        }
    }
}

