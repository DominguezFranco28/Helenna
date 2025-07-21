using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AgileJumpState : IState, IMovable
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private PlatformDetector _platformDetector;
    private float _moveSmoothTime = 0.05f; //ojo este valor, si es muy alto se bugea por el desplazamiento lento y colisiones
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _targetPosition; // Guardamos solo una vez
    public AgileJumpState (AgilePlayerBehaviour agilePlayerBehaviour,  AgileStateMachine agileStateMachine, PlatformDetector platformDetector)
    {
       this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
        this._platformDetector = platformDetector;
    }
    public void Enter()
    {
        Debug.Log("Entraste al estaod de SALTO");
        _targetPosition = _platformDetector.PlatFormPosition;
        //_agilePlayerBehaviour.transform.position = _platformDetector.PlatFormPosition;


    }

    public void Exit()
    {
        Debug.Log("Saliste del estado de SALTO");
        _agilePlayerBehaviour.StopMovement();
    }

    public void MoveTo(Vector2 direction)
    {
        Vector2 smoothPosition = Vector2.SmoothDamp(_agilePlayerBehaviour.transform.position, direction, ref _velocity, _moveSmoothTime); //mismo desplazamiento q caja
        _agilePlayerBehaviour.transform.position = smoothPosition;
    }


    public void Update()
    {
        MoveTo(_targetPosition);
        float distanceToTarget = Vector2.Distance(_agilePlayerBehaviour.transform.position, _platformDetector.PlatFormPosition);
        if (distanceToTarget < 0.1f) //mejor ais porque chequeando el false en PlatformPosition tenia delay
        {
            _agilePlayerBehaviour.transform.position = _platformDetector.PlatFormPosition; //force aca la pos porque se bugeaba un poco a;l borde
            _agilePlayerBehaviour.StopMovement();
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
        }
    }
}
