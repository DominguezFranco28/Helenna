using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildActionState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ChildTriggerDetector _actionDetector;
    private Collider2D _collider2D;


    public ChildActionState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ChildTriggerDetector detector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._actionDetector = detector;
    }

    public void Enter()
    {
        _childPlayerBehaviour.StopMovement();
        Debug.Log("Accionaste una palanca");
        //_USAR ANIMACION ACA
        // AGREGAR CLIP SFXManager.Instance.PlaySFX();
        _collider2D = _actionDetector.LevelCollider;
        IActiveable activate = _collider2D.gameObject.GetComponent<IActiveable>();
        if (activate != null)
        {
            activate.Activate();
            _actionDetector.CanActivate = false;
            //_collider2D.enabled = false;  probar mejor a apagar el collider solo del item seleccionado 

        }
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado : ACTION");
    }

    public void Update()
    {
        if (!_actionDetector.CanActivate)
        {
            _childStateMachine.TransitionTo(_childStateMachine.moveState);
        }
    }
}
