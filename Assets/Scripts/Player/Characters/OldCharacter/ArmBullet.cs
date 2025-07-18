using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseDefinition;

public class ArmBullet : MonoBehaviour
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _pushDistance = 5f;
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private Collider2D _armCol;  
    private ArmImpulser _armImpulser;
    private ImpulseType _impulseType;
    // Methods to set the reference from outside the script,
    // from the armImpulser when instantiating the arm.
    // I bring the direction and strength of the impulse to the ArmBUllet instantiation.
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
        _armCol = GetComponent<Collider2D>();

        Destroy(gameObject, 0.8f);
    }
    private void FixedUpdate()
    {
        _rb.velocity = _direction * _shotSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // maybe I should make a switch, since there is no more than 3 posible scenarios for the armbullet colission
        if (collision.gameObject.CompareTag("HookPoint"))
        {
            Debug.Log("Impact whit hook point!");
             Destroy(gameObject);
            if (_armImpulser != null)
            {
                //Distance before the 0 point of intact, so that it brakes a little earlier and the collisions do not overlap
                Vector2 impactPoint = collision.contacts[0].point;
                float stopDistance = 0.5f; 
                Vector2 stopPoint = impactPoint - _direction.normalized * stopDistance;
                _armImpulser.MovePlayerToAnchor(stopPoint, _impulseType);
                //I call the public armimpulser method to activate the movement. 
                //With the stoppoint parameter I make sure that it brakes a little before reaching the anchor
            }
        }
        else if ((collision.gameObject.CompareTag("Pushable")) && _impulseType == ImpulseType.Push)
        {
            Destroy(gameObject);            
            Debug.Log("Impact whit movableObject");
            var collisionMove =collision.gameObject.GetComponent<MovableObject>();
            Vector2 impactPoint = collision.contacts[0].point;
            // Determino la direccion en X e  Y, quiero evitar diagonales 
            Vector2 pushDir = _direction; //a la direccion se le asigna un nuevo valor de tipo mvector 2, pero restringido para eivtar diagonales
            if (Mathf.Abs(pushDir.x) > Mathf.Abs(pushDir.y))
            {
                pushDir = new Vector2(Mathf.Sign(pushDir.x), 0); // Solo eje X
            }
            else
            {
                pushDir = new Vector2(0, Mathf.Sign(pushDir.y)); // Solo eje Y
            }



            Vector2 pushTarget = (Vector2)collision.transform.position + pushDir * _pushDistance;
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
            // // Determino la direccion en X e  Y, quiero evitar diagonales 
            Vector2 pushDir = _direction; //a la direccion se le asigna un nuevo valor de tipo mvector 2, pero restringido para eivtar diagonales
            if (Mathf.Abs(pushDir.x) > Mathf.Abs(pushDir.y))
            {
                pushDir = new Vector2(Mathf.Sign(pushDir.x), 0); // Solo eje X
            }
            else
            {
                pushDir = new Vector2(0, Mathf.Sign(pushDir.y)); // Solo eje Y
            }


            Vector2 pushTarget = (Vector2)collision.transform.position - pushDir * _pushDistance; //Lo mismo pero paso en negativo la direccion, para que vaya hacia el jugador
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
