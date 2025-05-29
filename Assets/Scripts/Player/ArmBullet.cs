using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseDefinition;

public class ArmBullet : MonoBehaviour
{
    private PlayerBehaviour _playerMovement;
    private StateMachine _stateMachine;
    private Rigidbody2D _rb;
    private MovableObject _movableObject; 
    
    [SerializeField] private float _shotSpeed;
    public Vector2 _direction;
    private Collider2D _armCol;
    private Collider2D _playerCol;
    
    private ArmImpulser _armImpulser;
    private ImpulseType _impulseType;

    // Métodos para setear la referencia desde afuera del script, desde el armImpulser al instanciar el brazo. Le traigo a la instanciacion del bulletArm la direccion yt fuerza del impulso.
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }
    public void SetImpulseForce(ArmImpulser impulser)
    {
        _armImpulser = impulser;
    }
    public void SetImpulseType(ImpulseType type)
    {
        _impulseType = type;
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _armCol = GetComponent<Collider2D>();;
        Destroy(gameObject, 2f);
    }
    private void FixedUpdate()
    {
        _rb.velocity = _direction * _shotSpeed;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HookPoint"))
        {
            Debug.Log("Impacto con un punto de anclaje!");
             Destroy(gameObject);
            // Avisar al TelekinesisForce que hubo impacto
            if (_armImpulser != null)
            {
                Vector2 impactPoint = collision.contacts[0].point;
                float stopDistance = 0.5f;  //Distancia antes del punto 0 de intacto, para que frene un poco antes y no se me solapen las colisiones
                Vector2 stopPoint = impactPoint - _direction.normalized * stopDistance;
                _armImpulser.MovePlayerToAnchor(stopPoint, _impulseType); //llamo al metodo pulico del force par aactivar el desplazamiento.Con el parametro stoppoint me aseguro que frene un poquito antes de llegar al anclaje
            }
            // Aplicar mas logica? Minimo la animaicon del gancho
        }
        else if ((collision.gameObject.CompareTag("Pushable")) && _impulseType == ImpulseType.Push)
        {

            Destroy(gameObject);            
            Debug.Log("Impactaste con un objeto movible");
            var collisionMove =collision.gameObject.GetComponent<MovableObject>();
            Vector2 impactPoint = collision.contacts[0].point;
            float pushDistance = 5f;
            Vector2 pushTarget = (Vector2)collision.transform.position + _direction * pushDistance;
            Collider2D targetCol = collisionMove.GetComponent<Collider2D>();
            if (targetCol != null && _armCol != null)
            {
                Physics2D.IgnoreCollision(_armCol, targetCol);
            }
            collisionMove.MoveTo(pushTarget);
        }
        else if ((collision.gameObject.CompareTag("Pushable")) && _impulseType == ImpulseType.Pull)
        {

            Destroy(gameObject);
            Debug.Log("Impactaste con un objeto movible");
            var collisionMove = collision.gameObject.GetComponent<MovableObject>();
            Vector2 impactPoint = collision.contacts[0].point;
            float pushDistance = 5f;
            Vector2 pushTarget = (Vector2)collision.transform.position - _direction * pushDistance; //Lo mismo pero paso en negativo la direccion, para que vaya hacia el jugador
            Collider2D targetCol = collisionMove.GetComponent<Collider2D>();
            if (targetCol != null && _armCol != null)
            {
                Physics2D.IgnoreCollision(_armCol, targetCol);
            }
            collisionMove.MoveTo(pushTarget); 
        }

        Destroy(gameObject);            
    }

 }
