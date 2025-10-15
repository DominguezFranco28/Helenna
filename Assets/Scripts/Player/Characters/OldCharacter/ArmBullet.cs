using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseDefinition;

public class ArmBullet : MonoBehaviour
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _pushDistance = 5f;
    [SerializeField] private float _lifeTime = 1f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _direction;
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private Transform _parentMovable; //para hacer padre a los puntos de anclajemovibles
    private ImpulseType _impulseType;

    [Header("Line Prefab")]
    [SerializeField] private GameObject _armLinePrefab;
    private ArmLineController _armLineInstance;
    private Transform _startPoint;
    private bool _isRetracting = false;
    private Vector2 _retractTarget;



    // Methods to set the reference from outside the script,
    // from the armImpulser when instantiating the arm.
    // I bring the direction and strength of the impulse to the ArmBUllet instantiation.
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
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
    public void SetStartTransform(Transform start)
    {
        _startPoint = start;
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _oldPlayerBehaviour = FindObjectOfType<OldPlayerBehaviour>(); //podria pasarlo por parametro como el impulseforce
        _animator = GetComponent<Animator>();
        StartCoroutine(AutoDestroy());
    }

    private void Start() //EN STAR XQ EN AWAKE PUEDE INSTANCIARSE NULL 
    {
        
        if (_armLinePrefab != null && _startPoint != null)
        {
            GameObject lineObj = Instantiate(_armLinePrefab);
            _armLineInstance = lineObj.GetComponent<ArmLineController>();
            _armLineInstance.AssignTarget(_startPoint.position, transform);
        }
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);
        // Pull: inicia retracción visual, no destruye todavia 
        StartRetract();
        if (!_isRetracting)
            Destroy(gameObject);
    }
    private void DestroyLine()
    {
        if (_armLineInstance != null)
        {
            _armLineInstance.CancelLine();
            _armLineInstance = null;
        }
    }

    private void OnDestroy()
    {
        //reseteo todas las banderas relacionadas al brazo lanzado UNA VEZ DESTRUIDO. Este se destruye automaticamente si no queda anclado
        //tiempo de vida del disparo ajustable desde inspector  
        _oldPlayerBehaviour.ArmPulled = false;
        _oldPlayerBehaviour.ArmRelease = false;
        _parentMovable = null;
        _oldPlayerBehaviour.SetMovementEnabled(true); //habilito el movimiento del jugador cuando se destruye elb razo
                                                      // Pull: inicia retracción visual, no destruye todavia 
        DestroyLine();
    }

    private void Update()
    {
        if (_isRetracting)
        {
            float step = _shotSpeed * 2 * Time.deltaTime; //en pull, la retraccion es mas rapida que el disparo para que no se bugeee cuando inpacte con algo
            transform.position = Vector2.MoveTowards(transform.position, _retractTarget, step);

            if (Vector2.Distance(transform.position, _retractTarget) < 0.1f)
            {
                _isRetracting = false;
                DestroyLine();
                Destroy(gameObject);
            }
        }
    }
    private void FixedUpdate()
    {
        _rb.velocity = _direction * _shotSpeed;
        
        _animator.SetTrigger("IsShooting");
        _animator.SetFloat("Horizontal", _direction.x);
        _animator.SetFloat("Vertical", _direction.y);
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
                        SFXManager.Instance.PlaySFX(_oldPlayerBehaviour.GrabSFX);
                        break;

                    case ImpulseType.Pull: //si viene cn typePUll el objeto se atrae
                        HandlePushableCollision(collision, false);
                        SFXManager.Instance.PlaySFX(_oldPlayerBehaviour.GrabSFX);
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
       if (collision.gameObject.tag.ToLower().Contains("throwable") && _impulseType == ImpulseType.Pull)
        {
            Debug.Log("Impact whit player REX");
            // Destroy the bullet if it collides with the player
            Transform parentTransform = collision.transform.parent; //PARENT porque la tag la tiene el objeto trigger de rex, no rex en si
            if (parentTransform != null)
            {           
                HandleDogPull(parentTransform);
                SFXManager.Instance.PlaySFX(_oldPlayerBehaviour.PullRexSFX);
            }
            StartRetract(); // bullet VUELVE  a harold
        }
    }
    private void HandlePushableCollision(Collision2D collision, bool isPush)
    {
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

        collisionMove.MoveTo(pushTarget);

        StartRetract();
    }
    private void HandleDogPull(Transform dogTransform)
    {
        //INTEGRAR ESTADO ACA///
            Rigidbody2D rb = dogTransform.GetComponent<Rigidbody2D>();
            AgilePlayerController controller = dogTransform.GetComponent<AgilePlayerController>();
            AgileTriggerDetector triggerDetector = dogTransform.GetComponentInChildren<AgileTriggerDetector>();
        if (rb != null)
            {
                controller.PullDirection(_startPoint.position); //la direccion de atraccion es hacia harold
            }
        //ojo aca, el bullet me devuelve rapido el control de harold y se me bugea un poco la pos pasada a rex
    }
    private void StartRetract()
    {
        if (_startPoint != null)
        {
            _retractTarget = _startPoint.position; // siempre hacia el origen
            _isRetracting = true;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
        }
    }
}
