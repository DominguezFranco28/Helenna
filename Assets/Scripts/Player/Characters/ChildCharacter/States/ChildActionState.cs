using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildActionState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _actionDetector;
    private Collider2D _collider2D;
    private float _actionDelay =1f;
    private float _actionTimer;
    private bool _delayCompleted;

    public ChildActionState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector detector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._actionDetector = detector;
    }

    public void Enter()
    {

        Debug.Log("Accionaste una palanca");
        //_USAR ANIMACION ACA
        ActivateLever();
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado : ACTION");
    }
    private void ActivateLever()
    {
        if (_actionDetector.LevelCollider)
        {
            _actionTimer = 0f;
            _delayCompleted = false;
            _childPlayerBehaviour.StopMovement();
            IActiveable activeable = _actionDetector.LevelCollider.GetComponent<IActiveable>();
            activeable.Activate();
            _actionDetector.CanActivate = true; //al final necesite volver a usar la palanca 
            _actionDetector.CanActivate = false; //al final necesite volver a usar la palanca 
            //animacion de cambio de lado con la palanca?

        }
    }
    public void Update()
    {
        // Wait for delay >> misma logica del dig del perro, pero le agrego el cd par amarcar el cambio de stado
        if (!_delayCompleted)
        {
            _actionTimer += Time.deltaTime;
            if (_actionTimer >= _actionDelay)
            {
                _delayCompleted = true;
                Debug.Log("End of delay");
            }
            return; // skip the update until delay is over
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateLever(); //resetea cada vez que apreta la e el ciclo de espera en la accion
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }

    }
}
