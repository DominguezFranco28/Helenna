using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseDefinition;

public class ArmBullet : MonoBehaviour
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _pushDistance = 5f;
    [SerializeField] private float _lifeTime = 1f;

    private LayerMask _collisionMask;//este guarda la mascara activa 
    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _direction;
    private Collider2D _armCol;  
    private ArmImpulser _armImpulser;
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private ImpulseType _impulseType;


    private Transform _parentMovable; //para hacer padre a los puntos de anclajemovibles
    private bool _isInHook = false; // bandera para saber si quedo enganchada
    private Vector2 _stopPoint;

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
    public void DetectVerticality(bool isHighGround)
    {
        if (isHighGround)
            gameObject.layer = LayerMask.NameToLayer("BulletHigh");
        else
            gameObject.layer = LayerMask.NameToLayer("BulletGround");
        // Cambiar layer segun la pos del jugador para detectar "veerticalidad", ajustado desde la matrix de unity gestiono las colisiones a gusto
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _armCol = GetComponent<Collider2D>();
        _oldPlayerBehaviour = FindObjectOfType<OldPlayerBehaviour>(); //podria pasarlo por parametro como el impulseforce
        _animator = GetComponent<Animator>();

        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);

        if (!_isInHook) // solo destruir si no esta enganchado
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //reseteo todas las banderas relacionadas al brazo lanzado UNA VEZ DESTRUIDO. Este se destruye automaticamente si no queda anclado
        //tiempo de vida del disparo ajustable desde inspector  
        _isInHook = false; // Reset the flag to allow future hook impacts
        _stopPoint = Vector2.zero;
        _oldPlayerBehaviour.ArmPulled = false;
        _oldPlayerBehaviour.ArmRelease = false;
        _parentMovable = null;
        _oldPlayerBehaviour.SetMovementEnabled(true); //habilito el movimiento del jugador cuando se destruye elb razo
        Debug.Log("ArmRelease " + _oldPlayerBehaviour.ArmRelease);

    }
    private void FixedUpdate()
    {
        Debug.Log("ArmRelease " + _oldPlayerBehaviour.ArmRelease);
        _rb.velocity = _direction * _shotSpeed;

        //parametro de direccion tomado de la pos de mouse, no de inputs
        
        _animator.SetTrigger("IsShooting");
        _animator.SetFloat("Horizontal", _direction.x);
        _animator.SetFloat("Vertical", _direction.y);
        if (_isInHook && _oldPlayerBehaviour.ArmPulled)
            {
               Debug.Log("THE ATTRACTION IS ACTIVATED");
            //stopPoint dinamico, para tener en cuenta la posiconm actual del HookPoint si se mueve.
            _stopPoint = (Vector2)_parentMovable.position - _direction.normalized * 0.5f;
            _armImpulser.MovePlayerToAnchor(_stopPoint, ImpulseType.Pull); //activo el reposicionamiento del jugador
               Destroy(gameObject); // Destroy the bullet after pulling the player
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HookPoint") && !_isInHook) 
        {
            Debug.Log("Impact whit hook point!");
            _isInHook = true; // Set the flag to true to prevent multiple hook impacts
            _parentMovable = collision.transform; // Get the parent transform of the hook point
            transform.SetParent(_parentMovable,true); //true para mantener la pos globan en el momento del enganche 

            // fuerzo el eje Z a 0 xq a veces se me iba a 58 (idk) y generaba problemas de visibilidad
            Vector3 fixedPosition = transform.position;
            fixedPosition.z = 0f;
            transform.position = fixedPosition;
            if (_rb != null)  
            {

                // freno el Rigidbody por completo
                _rb.velocity = Vector2.zero;
                _rb.isKinematic = true; // tuve que desactivar el rb porque interferia con el mov del padre (bug visual si el Hook se movia x codigo)}

            }
            _shotSpeed = 0f; // Stop the bullet's movement
            _armCol.enabled = false; // Disable the collider to prevent further collisions

        }
        // maybe I should make a switch
        else if ((collision.gameObject.CompareTag("Pushable")) && _impulseType == ImpulseType.Push)
        {
            Destroy(gameObject); // Destroy the bullet if it collides with a pushable object
            Debug.Log("Impact whit movableObject");
            var collisionMove = collision.gameObject.GetComponent<MovableObject>();
            Vector2 impactPoint = collision.contacts[0].point;
            // Si el objeto es movible, lo empujo en la direccion del disparo
            // Determino la direccion en X e  Y, a la direccion se le asigna un nuevo valor de tipo vector 2, pero restringido para evitar diagonales
            Vector2 pushDir = _direction; 
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
            Destroy(gameObject); // Destroy the bullet if it collides with a pushable object
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
        else
        {
            Destroy(gameObject); // Destroy the bullet if it collides with anything else
        }
    }
 }
