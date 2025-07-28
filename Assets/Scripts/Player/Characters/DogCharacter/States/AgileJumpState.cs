using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AgileJumpState : IState, IMovable, IFixedUpdate
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private PlatformDetector _platformDetector;
    private float _moveSmoothTime = 0.07f; //ojo este valor, si es muy alto se bugea por el desplazamiento lento y colisiones
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _targetPosition; // Guardamos solo una vez

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
        _agilePlayerBehaviour.Animator.SetBool("Jump", true);
        SFXManager.Instance.PlaySFX(_agilePlayerBehaviour.JumpSFXClip);
    }

    public void Exit()
    {
        Debug.Log("Saliste del estado de SALTO");
        _agilePlayerBehaviour.Animator.SetBool("Jump", false);
        _agilePlayerBehaviour.RestartCooldown(); //cada vez que sale del salto, resetea el delay y vuelve a correr (configurable desde el inspecto)
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

        //Comienza logica FISICA de salto.
       
        //seteo la anim de salto segun inputs dentro del blend tree. Logica similar a la del moveState, pero necesite hacerlo de aca para que el jugador no pueda forzar la anim incorrecta
        MoveTo(_targetPosition);
        _agilePlayerBehaviour.PlayerCollider.enabled = false; 
        //al igual que harold, tuve que apagar el collider para que no haga cosas extranas con las colisiones. No tengo enemigos dinamicos ni objetos de dano asi que no hayd rmaa

        float distance = Vector2.Distance(_agilePlayerBehaviour.Rigidbody2D.position, _targetPosition);
        if (distance < 0.1f)
        {
            _agilePlayerBehaviour.Rigidbody2D.position = _targetPosition; // fuerza la posición final exacta
            _agilePlayerBehaviour.StopMovement();
            _agilePlayerBehaviour.PlayerCollider.enabled = true;//vuelve a prender el colider
            if (_pickedObject != null) //si desde item state recibe el objeto como parametro del metodo Object, devuelve a ese estado para poder agarrar y soltar objetos luego del salto
            {
                
                _agileStateMachine.TransitionTo(_agileStateMachine.itemState);
            }
            else
                _agileStateMachine.TransitionTo(_agileStateMachine.moveState);
        }
    }
}
