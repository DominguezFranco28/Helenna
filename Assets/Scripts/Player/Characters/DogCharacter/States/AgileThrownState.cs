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
    public float throwSpeed = 10f; // unidades por segundo
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


        // Movimiento suave de A a B
        _agilePlayerBehaviour.transform.position = Vector2.MoveTowards(
            _agilePlayerBehaviour.transform.position,
            _targetPosition,
            throwSpeed * Time.fixedDeltaTime
        );

        // Cuando llega al destino
        if (Vector2.Distance(_agilePlayerBehaviour.transform.position, _targetPosition) < 0.01f)
        {
            _playerController.FinishThrow();
        }
    }

    public void Update()
    {
    }
}
