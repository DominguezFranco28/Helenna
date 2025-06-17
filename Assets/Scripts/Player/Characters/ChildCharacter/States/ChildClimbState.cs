using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildClimbState : IState
    
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    private ClimbDetector _climbDetector;
    private Collider2D _ignoredClimbable;

    public ChildClimbState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine, ClimbDetector climbDetector)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
        this._climbDetector = climbDetector;
    }
    public void Enter()
    {
        Debug.Log("Entraste al modo CHILD CLIMB");
        //_childPlayerBehaviour.StopMovement();
        if (_climbDetector.Climbable != null)
        {
            _childPlayerBehaviour.PlayerCollider.enabled = false;
            _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.ClimbSpeed);
            _childPlayerBehaviour.Animator.SetBool("isClimbing", true);
            SFXManager.Instance.PlayLoop(_childPlayerBehaviour.ClimbSFX);


        }
    }

    public void Exit()
    {
        Debug.Log("Saliendo de estado Climb");
        // Restaurar colisiones al salir de la escalada
        if (_ignoredClimbable != null)
        {
            _childPlayerBehaviour.transform.position += Vector3.up * 0.05f; //desplazo un pcoo haacia arriba al jugador apra que de sensacion de salto despues de escalada
            _ignoredClimbable = null;
        }
        _childPlayerBehaviour.Animator.SetBool("isClimbing", false);
        _childPlayerBehaviour.PlayerCollider.enabled = true;
        _childPlayerBehaviour.StopMovement();
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.DefaultSpeed);
        SFXManager.Instance.StopLoop();
        //dsactivar animación: _childPlayerBehaviour.Animator.SetBool("isClimbing", false);
    }


    public void Update()
    {

        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 climbVelocity = new Vector2(0f, vertical * _childPlayerBehaviour.ClimbSpeed);
        _childPlayerBehaviour.SetMovementInput(climbVelocity);
        _childPlayerBehaviour.SetSpeed(_childPlayerBehaviour.ClimbSpeed);

        // Si ya no puede escalar (por colisión u otra cosa), volver a Idle, podria agregar una tecleada.
        if (!_climbDetector.canClimb || Input.GetKeyDown(KeyCode.E))
        {
            _childStateMachine.TransitionTo(_childStateMachine.idleState);
        }
    }
}
