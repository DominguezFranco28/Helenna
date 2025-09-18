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

    public float throwSpeed = 15f;
    public float maxThrowDistance = 10f;
    public float targetRetreatOffset = 0.5f; // cuanto retrocedemos si hay agua parcial
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
        // Calculamos el target final
        _targetPosition = _startPosition + _agilePlayerBehaviour.PendingThrowDirection.normalized * maxThrowDistance;


        // LIMPIEZA DE VELOCIDAD PREVIA
        _agilePlayerBehaviour.Rigidbody2D.velocity = Vector2.zero;
        _agilePlayerBehaviour.Rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        _throwCompleted = false;
    }

    public void Exit()
    {
        Debug.Log("You exited the state: AGILE THROW");
        _agilePlayerBehaviour.PlayerCollider.enabled = true;
    }

    public void FixedUpdate()
    {

    //    //podria poner un delay aca antes de empezar a mover al perro, para que de la sensacion de que lo lanzan y despues vuela
    //    // Movimiento suave de A a B
        Rigidbody2D rb = _agilePlayerBehaviour.Rigidbody2D;

        Vector2 throwDir = _agilePlayerBehaviour.PendingThrowDirection.normalized;
        // direccion normalizada hacia el target
        // Movimiento constante hacia la dirección de lanzamiento
        rb.velocity = throwDir * throwSpeed;



        //podria agregar un raycast de seguridad para evitar que siga el empujon si hay agua delante 

        // distancia al target
        float distance = Vector2.Distance(rb.position, _targetPosition);
        if (_agilePlayerBehaviour.HoleDetector.IsInWater)
        {
            _agilePlayerBehaviour.PlayerCollider.enabled = false;
        }
        float pushThreshold = 1f; // distancia a partir de la cual se le da un empujoncito final para que no se quede justo en el borde
        if (distance < pushThreshold && !_throwCompleted)
        {
            _throwCompleted = true;
            float finalPushStrength = 2f; // ajustable
            rb.AddForce(throwDir * finalPushStrength, ForceMode2D.Impulse);
            // Corrección extra para bordes: mueve ligeramente a Rex fuera del borde
            rb.position += throwDir * 0.2f; // ajustable
        }

            // fin del throw solo si ya no esta en el agua
            if (_throwCompleted && !_agilePlayerBehaviour.HoleDetector.IsInWater)
            {
                rb.velocity = Vector2.zero;
                _playerController.FinishThrow();
            }
        }
    public void Update()
    {
        //anims
    }
}


    //    _agilePlayerBehaviour.Rigidbody2D.position = Vector2.MoveTowards(
    //        _agilePlayerBehaviour.transform.position,
    //        _targetPosition,
    //        throwSpeed * Time.fixedDeltaTime
    //    );
    //    Vector2 currentPos = _agilePlayerBehaviour.transform.position;

        //    // Cuando llega al destino. Ojo con el valor hardcodeado porque si es muy chico capaz no lo detecta en horizontales
        //    if (Vector2.Distance(currentPos, _targetPosition) < 0.5f || _agilePlayerBehaviour.HoleDetector.IsInWater) //
        //    {
        //        if (!_throwCompleted)
        //        {
        //            _throwCompleted = true; // Llegó al destino
        //        }

        //        // Solo finalizar throw si ya no está en el agua
        //        if (!_agilePlayerBehaviour.HoleDetector.IsInWater)
        //        {
        //            _playerController.FinishThrow();
        //        }
        //    }

