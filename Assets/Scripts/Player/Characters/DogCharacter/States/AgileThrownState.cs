using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgileThrownState : IState, IFixedUpdate
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private AgilePlayerController _playerController;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    public float throwSpeed = 20f; // unidades por segundo
    public AgileThrownState(AgilePlayerBehaviour player, AgileStateMachine agileStateMachine, AgilePlayerController playerController)
    {
        this._agilePlayerBehaviour = player;
        this._agileStateMachine = agileStateMachine;
        _playerController = playerController;
        //le termine pasando en el constructor el player controller para poder avisarle cuando termina el throw desde el metodo de esa clase
    }

    public void Enter()
    {
        Debug.Log("You entered the state: AGILE THROW");
        _startPosition = _agilePlayerBehaviour.transform.position;
        //target posiicion harcodeada
        _targetPosition = _startPosition + _agilePlayerBehaviour.PendingThrowDirection * 10f;
    }

    public void Exit()
    {
        Debug.Log("You exited the state: AGILE THROW");
    }

    public void FixedUpdate()
    {

        //podria poner un delay aca antes de empezar a mover al perro, para que de la sensacion de que lo lanzan y despues vuela

        // Movimiento suave de A a B
        _agilePlayerBehaviour.transform.position = Vector2.MoveTowards(
            _agilePlayerBehaviour.transform.position,
            _targetPosition,
            throwSpeed * Time.fixedDeltaTime
        );

        // Cuando llega al destino
        if (Vector2.Distance(_agilePlayerBehaviour.transform.position, _targetPosition) < 0.01f)
        {

            //tengo q discriminar de alguna forma si esta en tierra o agua para que se siga movimiendo hasta que no pueda caer en el vacio
            //lo de abajo fue un buen acercamiento


            if (_agilePlayerBehaviour.IsGrounded)
            {
                // Está en suelo/plataforma según el modo
                 _playerController.FinishThrow();

            }
            else
            {
               // seguir avanzando en la misma direccion hasta encontrar suelo
                _agilePlayerBehaviour.transform.position += (Vector3)_targetPosition * throwSpeed;

                if (_agilePlayerBehaviour.IsGrounded)
                {
                    _playerController.FinishThrow();
                }
            }
        }
    }

    public void Update()
    {
        if (_agilePlayerBehaviour.IsGrounded)
        {
            // Está en suelo/plataforma según el modo
            // Hacés lo que corresponda en tu lógica de throw
        }
        else
        {
            // Está en el aire
        }
    }
}
