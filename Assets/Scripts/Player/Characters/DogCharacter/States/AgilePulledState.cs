using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AgilePulledState : IState, IFixedUpdate
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private AgilePlayerController _playerController;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private bool _pullCompleted = false;

    public float _pullSpeed = 20f;

    //HAROLD TIENE UNO EN SU THROW TAMB
    private float _throwDelay = 1f;
    private float _throwTimer;
    private bool _delayCompleted;
    public AgilePulledState(AgilePlayerBehaviour player, AgileStateMachine agileStateMachine, AgilePlayerController playerController)
    {
        this._agilePlayerBehaviour = player;
        this._agileStateMachine = agileStateMachine;
        _playerController = playerController;
        //le termine pasando en el constructor el player controller para poder avisarle cuando termina el throw desde el metodo de esa clase
    }

    public void Enter()
    {
        Debug.Log("You entered the state: AGILE PULLED");
        _agilePlayerBehaviour.TriggerDetector.IgnoreWater(true); // desactivo el collider para que no choque con nada mientras lo agarra harold
        _startPosition = _agilePlayerBehaviour.transform.position;
        _targetPosition = _agilePlayerBehaviour.PendingPulledDirection;
        _pullCompleted = false;
    }

    public void Exit()
    {
        Debug.Log("You exited the state: AGILE PULLED");
        // fuerzo terminar el pull si no se completó (si el desplazamiento era largo y harold se movia, se le rompia la maquina de estados a rex)
        if (!_pullCompleted)
        {
            _pullCompleted = true;
            EndPull(_agilePlayerBehaviour.Rigidbody2D);
        }
    }

    public void FixedUpdate()
    {

        // if (!_delayCompleted) return; //si no termino el delay, que no ejecute el resto de la secuencia (en update para trabajar al mismo tiempo que harold)
        if (_pullCompleted) return; // nada que hacer si ya se completo
        Rigidbody2D rb = _agilePlayerBehaviour.Rigidbody2D;
                                                                          
        Vector2 pullDir = (_targetPosition - rb.position).normalized;           // dirección hacia el target
        // Movimiento constante hacia la dirección de lanzamiento
        rb.position = Vector2.MoveTowards(rb.position, _targetPosition, _pullSpeed * Time.fixedDeltaTime);
        //rb.velocity = pullDir * _pullSpeed;

        float distanceToTarget = Vector2.Distance(rb.position, _targetPosition);
        if (distanceToTarget < 0.2f)
        {
            if (!_pullCompleted)
            {
                _pullCompleted = true;
                EndPull(rb);
            }
        }


    }
    private void EndPull(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
        //RENUEVO EL COLLIDER CUANDO TERMINA EL PULLS
        _agilePlayerBehaviour.TriggerDetector.IgnoreWater(false);
        _playerController.FinishPull();
    }
    public void Update()
    {
        //anims
        //if (!_delayCompleted)
        //{
        //    _throwTimer += Time.deltaTime;
        //    if (_throwTimer >= _throwDelay)
        //    {
        //        _delayCompleted = true;
        //        Debug.Log("End of delay");

        //    }
        //    return; // skip the update until delay is over
        //}
    }
}

