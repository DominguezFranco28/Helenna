using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AgileJumpState : IState, IMovable, IFixedUpdate
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private PlatformDetector _platformDetector;
    private float _moveSmoothTime = 0.05f; //ojo este valor, si es muy alto se bugea por el desplazamiento lento y colisiones
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _targetPosition; // Guardamos solo una vez
    private Rigidbody2D _rb2D;
    private Collider2D _collider2D;
    public AgileJumpState (AgilePlayerBehaviour agilePlayerBehaviour,  AgileStateMachine agileStateMachine, PlatformDetector platformDetector)
    {
       this._agilePlayerBehaviour = agilePlayerBehaviour;
        this._agileStateMachine = agileStateMachine;
        this._platformDetector = platformDetector;
        _rb2D = agilePlayerBehaviour.GetComponent<Rigidbody2D>();
        _collider2D = agilePlayerBehaviour.GetComponent<Collider2D>();
        
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

    public void MoveTo(Vector2 direction) //tendria que revisar y refactorizar esto, no uso fixed update ni mov con rigidbody, como tuve que arreglar con Harold
    {
        Vector2 smoothPos = Vector2.SmoothDamp(_rb2D.position, direction, ref _velocity, _moveSmoothTime);
        _rb2D.MovePosition(smoothPos);
    }


    public void Update() { }
    public void FixedUpdate()
    {
        MoveTo(_targetPosition);
        _collider2D.enabled = false; //al igual que harold, tuve que apagar el collider para que no haga cosas extranas. Me ahorro el tema de tener que ajustar a mano con que capaz puede interactuar o nno
        float distance = Vector2.Distance(_rb2D.position, _targetPosition);
        if (distance < 0.1f)
        {
            _rb2D.position = _targetPosition; // fuerza la posición final exacta
            _agilePlayerBehaviour.StopMovement();
            _collider2D.enabled = true;
            _agileStateMachine.TransitionTo(_agileStateMachine.idleState);
        }
    }
}
