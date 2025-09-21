using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AgileThrownState : IState, IFixedUpdate
{
    private AgilePlayerBehaviour _agilePlayerBehaviour;
    private AgileStateMachine _agileStateMachine;
    private AgilePlayerController _playerController;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private bool _throwCompleted = false;

    public float throwSpeed = 20f;
    public float maxThrowDistance = 6f;

    //HAROLD TIENE UNO EN SU THROW TAMB
    private float _throwDelay = 1f;
    private float _throwTimer;
    private bool _delayCompleted;
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
        _targetPosition = _startPosition + _agilePlayerBehaviour.PendingThrowDirection.normalized * maxThrowDistance;
        _throwCompleted = false;
        _throwTimer = 0f;
        _delayCompleted = false;
        // LIMPIEZA DE VELOCIDAD PREVIA
        _agilePlayerBehaviour.Rigidbody2D.velocity = Vector2.zero;
//        _agilePlayerBehaviour.Rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

    }

    public void Exit()
    {
        Debug.Log("You exited the state: AGILE THROW");
        _agilePlayerBehaviour.TriggerDetector.IgnoreWater(false);
    }

    public void FixedUpdate()
    {

        if (!_delayCompleted) return; //si no termino el delay, que no ejecute el resto de la secuencia (en update para trabajar al mismo tiempo que harold)

        Rigidbody2D rb = _agilePlayerBehaviour.Rigidbody2D;
        // direccion normalizada hacia el target
        Vector2 throwDir = _agilePlayerBehaviour.PendingThrowDirection.normalized;
        // Movimiento constante hacia la dirección de lanzamiento
        rb.velocity = throwDir * throwSpeed;

        bool inWater = _agilePlayerBehaviour.TriggerDetector.IsInWater;
        bool waterAhead = _agilePlayerBehaviour.TriggerDetector.WaterAhead;
        float distanceToTarget = Vector2.Distance(rb.position, _targetPosition);


        // si el target justo termina en el agua, o tenga agua delante que siga el desplazamiento
        if (distanceToTarget < 0.5f && (inWater || waterAhead))
        {
            // leve extension de la pos del throw para salir del agua 
            _targetPosition = rb.position + throwDir * 0.2f;
        }

        // termina throw solo si no hay agua por delante, ni esta en el agua y llegó al target
        if (!waterAhead  && !inWater && distanceToTarget < 0.5f)
        {
            if (!_throwCompleted)
            {
                _throwCompleted = true;
                EndThrow(rb);
            }
        }
        // ignorar colision con la layer del agua mientras se esta en el throw
        if (inWater || waterAhead)
            _agilePlayerBehaviour.TriggerDetector.IgnoreWater(true);
        else
            _agilePlayerBehaviour.TriggerDetector.IgnoreWater(false);

    }
    private void EndThrow(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
        _agilePlayerBehaviour.TriggerDetector.IgnoreWater(false);
        _playerController.FinishThrow();
    }
    public void Update()
    {
        //anims
        if (!_delayCompleted)
        {
            _throwTimer += Time.deltaTime;
            if (_throwTimer >= _throwDelay)
            {
                _delayCompleted = true;
                Debug.Log("End of delay");

            }
            return; // skip the update until delay is over
        }
    }
}

