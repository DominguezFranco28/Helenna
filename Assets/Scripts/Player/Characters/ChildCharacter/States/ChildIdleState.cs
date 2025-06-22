using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildIdleState : IState
{
    private ChildPlayerBehaviour _childPlayerBehaviour;
    private ChildStateMachine _childStateMachine;
    public ChildIdleState(ChildPlayerBehaviour childPlayerBehaviour, ChildStateMachine childStateMachine)
    {
        this._childPlayerBehaviour = childPlayerBehaviour;
        this._childStateMachine = childStateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entraste al estado: IDLE CHILD");
        _childPlayerBehaviour.StopMovement(); //Freezeamos al pj al entrar a este estado.
        _childPlayerBehaviour.SetMovementEnabled(true);
        //Podria setear un trigger de entrada a la animación de idle aca, por ejemplo exsconder el arma
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado: IDLE");
        //Frenar animación, o a lo mejor hacer una animacion de exit como que levanta polvillo o algo.
    }

    public void Update()
    {
        //Conducta de movimiento ligeramente diferente al resto de players, ya que la niña implementa una "falsa_ verticalidad" con la escalada.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            _childStateMachine.TransitionTo(_childStateMachine.moveState);
            return;
        }

        // Detectar si puede escalar
        if (_childPlayerBehaviour.ClimbDetector.canClimb && Mathf.Abs(vertical) > 0.1f)
        {
            _childStateMachine.TransitionTo(_childStateMachine.climbState);
            return;
        }
    }
}
   
