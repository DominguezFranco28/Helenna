using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState :  IState
{
    private PlayerBehaviour _playerMovement;
    private StateMachine _stateMachine;


    public IdleState(PlayerBehaviour player, StateMachine stateMachine) // Doble constructor, porque no hereda de monobehavoir
    {
        this._playerMovement = player;
        this._stateMachine = stateMachine;
    }
    public void Enter()
    {
        Debug.Log("Entraste al estado: IDLE");
        _playerMovement.SetMovementInput(Vector2.zero); //Freezeamos al pj al entrar a este estado.
        //Podria setear un trigger de entrada a la animación de idle aca, por ejemplo exsconder el arma
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado: IDLE");
        //Frenar animación, o a lo mejor hacer una animacion de exit como que levanta polvillo o algo.
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.magnitude > 0.01f)
        {
            _stateMachine.TransitionTo(_stateMachine.moveState); //Pasaje a estado de movimiento
        }

        _playerMovement.SetMovementInput(Vector2.zero);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _stateMachine.TransitionTo(_stateMachine.telekinesisState); //Pasaje a estado de movimiento
        }
    }
}

