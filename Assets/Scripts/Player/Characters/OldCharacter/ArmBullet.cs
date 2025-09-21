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
        _rb.velocity = _direction * _shotSpeed;
        
        _animator.SetTrigger("IsShooting");
        _animator.SetFloat("Horizontal", _direction.x);
        _animator.SetFloat("Vertical", _direction.y);
        //if (_isInHook && _oldPlayerBehaviour.ArmPulled)
        //    {
        //       Debug.Log("THE ATTRACTION IS ACTIVATED");
        //    //stopPoint dinamico, para tener en cuenta la posiconm actual del HookPoint si se mueve //creo que alfinal no va a ser necesario
        //    _stopPoint = (Vector2)_parentMovable.position - _direction.normalized * 0.5f;
        //    _armImpulser.MovePlayerToAnchor(_stopPoint, ImpulseType.Pull); //activo el reposicionamiento del jugador
        //       Destroy(gameObject); // Destroy the bullet after pulling the player
        //    }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        switch (tag)
        {
            //case "HookPoint": //manejo de colision con puntos de anclaje, me quedo obsoleto conm el anchordetector
            //    HandleHookPointCollision(collision);
            //    break;

            case "Pushable": //manejo de colision con objetos empujables 
                switch (_impulseType)
                {
                    case ImpulseType.Push:
                        HandlePushableCollision(collision, true);
                        break;

                    case ImpulseType.Pull: //si viene cn typePUll el objeto se atrae
                        HandlePushableCollision(collision, false);
                        break;
                }
                break;

             default:
                //si colisiona con cualquier otra cosa, que se destruya.
                 Destroy(gameObject);
                break;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag.ToLower().Contains("throwable"))
        {
            Debug.Log("Impact whit player REX");
            Destroy(gameObject); // Destroy the bullet if it collides with the player
            Transform parentTransform = collision.transform.parent; //PARENT porque la tag la tiene el objeto trigger de rex, no rex en si
            if (parentTransform != null)
            {           
                HandleDogThrow(parentTransform);
            }
        }
    }
    //private void HandleHookPointCollision(Collision2D collision)
    //{
    //    Debug.Log("Impact with hook point!");
    //    _isInHook = true;
    //    _parentMovable = collision.transform;
    //    transform.SetParent(_parentMovable, true);

    //    Vector3 fixedPosition = transform.position;
    //    fixedPosition.z = 0f;
    //    transform.position = fixedPosition;

    //    if (_rb != null)
    //    {
    //        _rb.velocity = Vector2.zero;
    //        _rb.isKinematic = true;
    //    }

    //    _shotSpeed = 0f;
    //    _armCol.enabled = false;
    //}
    private void HandlePushableCollision(Collision2D collision, bool isPush)
    {
        Destroy(gameObject);
        Debug.Log("Impact with movable object");

        var collisionMove = collision.gameObject.GetComponent<MovableObject>();

        // direccion solo en eje principal (sin diagonales)
        Vector2 dir = _direction;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            dir = new Vector2(Mathf.Sign(dir.x), 0);
        else
            dir = new Vector2(0, Mathf.Sign(dir.y));

        //Operador ternario aca, si ispush es verdaderoo se mueve en la direccion del disparo, si es pull se mueve en sentido contrario
        Vector2 pushTarget = (Vector2)collision.transform.position +
                             (isPush ? dir : -dir) * _pushDistance;
        

        //Collider2D targetCol = collisionMove.GetComponent<Collider2D>();
        //if (targetCol != null && _armCol != null)
        //    Physics2D.IgnoreCollision(_armCol, targetCol);    //esata parte me quedo bosoleta porque ya destruyo el brazo nin bien colisiona, pero dejo de momento 

        collisionMove.MoveTo(pushTarget);
    }
    private void HandleDogThrow(Transform transform)
    {
        //INTEGRAR ESTADO ACA///
            Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
            Collider2D col = transform.GetComponent<Collider2D>();
            AgileTriggerDetector holeDetector = transform.GetComponentInChildren<AgileTriggerDetector>();
            if (rb != null)
            {
                holeDetector.IsBeeingPulled = true;
                rb.MovePosition(_oldPlayerBehaviour.transform.position);
            }
            holeDetector.IsBeeingPulled = false;       
    }
}
