using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AgileJumpState : IState, IMovable, IFixedUpdate
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private PlatformDetector _platformDetector;
    private float _moveSmoothTime = 0.2f; //ojo este valor, si es muy alto se bugea por el desplazamiento lento y colisiones
    private Vector2 _velocity = new Vector2 (1.5f,1.5f);
    private Vector2 _targetPosition; // Guardamos solo una vez
    private Vector2 _lastInput;
    private GameObject _pickedObject = null;

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
        if (_targetPosition == Vector2.zero)
        {//validacion porque empezo a hacer saltos raro si tenia item. Ahora si la paltaforma tiene un vector2 en zerio, cancela el salto.
            Debug.LogWarning("PlatFormPosition inválido al saltar");
        }
        //"EJjecuta la anim y sonido de salto"
        if (_agilePlayerBehaviour.IsGrounded)
        {
             _agilePlayerBehaviour.Animator.SetBool("Jump", true);
            SFXManager.Instance.PlaySFX(_agilePlayerBehaviour.JumpSFXClip);

        }
        // _lastInput = _agilePlayerBehaviour.LastMovementInput;
        //_agilePlayerBehaviour.Animator.SetFloat("Horizontal", _lastInput.x);
        //_agilePlayerBehaviour.Animator.SetFloat("Vertical", _lastInput.y);
        //_agilePlayerBehaviour.Animator.SetFloat("Speed", _lastInput.magnitude);
        //_agilePlayerBehaviour.SetMovementEnabled(false);
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado de SALTO");
        if (!_agilePlayerBehaviour.IsGrounded)
        {
        _agilePlayerBehaviour.Animator.SetBool("Jump", false);
        _agilePlayerBehaviour.RestartCooldown(); //cada vez que sale del salto, resetea el delay y vuelve a correr (configurable desde el inspecto)

        }
    }

    public void MoveTo(Vector2 direction) //tendria que revisar y refactorizar esto, no uso fixed update ni mov con rigidbody, como tuve que arreglar con Harold
    {
        Vector2 smoothPos = Vector2.SmoothDamp(_agilePlayerBehaviour.Rigidbody2D.position, direction, ref _velocity, _moveSmoothTime);
        _agilePlayerBehaviour.Rigidbody2D.MovePosition(smoothPos);
    }


    public void Update() 
    {

    }
   private void UpdateAnimator()
    {

        Vector2 jumpDirection = (_targetPosition - _agilePlayerBehaviour.Rigidbody2D.position).normalized;

        _agilePlayerBehaviour.Animator.SetFloat("Horizontal", jumpDirection.x);
        _agilePlayerBehaviour.Animator.SetFloat("Vertical", jumpDirection.y);
    }

    public void Object (GameObject newPicked)
    {
        _pickedObject = newPicked;
    }
    public void FixedUpdate()
    {
        if (!_agilePlayerBehaviour.DelayCompleted) //si no termino el delay, que no ejecute el resto de la secuencia
            return;

        UpdateAnimator(); //logica de animacion del salto

        //Comienza logica FISICA de salto. Como dezplazo al pj dentro de esta funcion y no desde el update del behavour, tengo que gestionar el fixed update aca  tambien xq muevo su rigidpody
        //seteo la anim de salto segun inputs dentro del blend tree. Logica similar a la del moveState, pero necesite hacerlo de aca para que el jugador no pueda forzar la anim incorrecta
        MoveTo(_targetPosition);
        _agilePlayerBehaviour.PlayerCollider.isTrigger = true; 

        float distance = Vector2.Distance(_agilePlayerBehaviour.Rigidbody2D.position, _targetPosition);
        Debug.Log("dog jump distance: " + distance);
        
            
        if (distance <= 0.3f)
        {
            Debug.Log("dog reached destination");
            _agilePlayerBehaviour.Rigidbody2D.position = _targetPosition; // fuerza la posición final exacta
            _agilePlayerBehaviour.StopMovement();
            _agilePlayerBehaviour.PlayerCollider.isTrigger = false;//vuelve a prender el colider
            
            if (_pickedObject != null) //si desde item state recibe el objeto como parametro del metodo Object, devuelve a ese estado para poder agarrar y soltar objetos luego del salto
                _agileStateMachine.TransitionTo(_agileStateMachine.itemState);
            else
                _agileStateMachine.TransitionTo(_agileStateMachine.moveState);
        }
    }
}
