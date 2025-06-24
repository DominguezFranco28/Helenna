using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseState : IState
{
    private PlayerBehaviour _playerBehaviour;
    private StateMachine _stateMachine;
    private float _duration = 1f;
    private float _timer;

    public ImpulseState(PlayerBehaviour player, StateMachine stateMachine)
    {
        _playerBehaviour = player;
        _stateMachine = stateMachine;

    }
    public void Enter()
    {
        Debug.Log("Entraste al estado: IMPULSE");
        _timer = _duration;
        //_playerBehaviour.canMove = false;
        _playerBehaviour.StopMovement();
        _playerBehaviour.Animator.SetTrigger("IsImpulsing"); 


    }

    public void Exit()
    {
        //_playerMovement.canMove = true;
        Debug.Log("Saliste del estado: IMPULSE");

    }

    public void Update()
    {
        _timer -= Time.deltaTime; //ver si puedo ajustar logica de cds
        if (Input.GetMouseButtonDown(0)) // Click izquierdo = empuje
        {
            _playerBehaviour.PerformThrowArm(ImpulseType.Push);

            _stateMachine.TransitionTo(_stateMachine.idleState);
        }

        if (Input.GetMouseButtonDown(1)) // Click derecho = atracción
        {
            _playerBehaviour.PerformThrowArm(ImpulseType.Pull);
            _stateMachine.TransitionTo(_stateMachine.idleState);
        }
    }
  
}
