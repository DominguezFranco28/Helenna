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
    private bool _throwCompleted = false;
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
        Vector2 currentPos = _agilePlayerBehaviour.transform.position;

        // Cuando llega al destino. Ojo con el valor hardcodeado porque si es muy chico capaz no lo detecta en horizontales
        if (Vector2.Distance(currentPos, _targetPosition) < 0.2f && !_throwCompleted && !_agilePlayerBehaviour.HoleDetector.IsInWater)
        {
            _throwCompleted = true;
            _playerController.FinishThrow();
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
