using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerBehaviour : MonoBehaviour, IControllable
{

    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _lowSpeed;
    [SerializeField] private AudioClip _footstepsSFX;
    [SerializeField] private AudioClip _grabSFX;
    [SerializeField] private AudioClip _throwSFX;
    [SerializeField] private AudioClip _pullRexSFX;
    [SerializeField] private AudioClip _ziplineSFX;
    private float _auxSpeed;
    private bool _canMove; //manage from CharacterManager
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private ArmImpulser _armImpulser;
    private Vector2 _movementInput;

    private bool _isInControll;
    private bool _isRecoiling = false;
    private bool _armReleased = false; //para saber si el brazo fue liberado, para no repetir la animacion de recoil con el throw
    private bool _armPulled = false; //para saber si el brazo fue liberado, para no repetir la animacion de recoil con el throw
    private bool _isOnHighGround;
    private bool _unlockZipline = false; //DESBLOQUEO DE HABILIDAD DE USAR TIROLESA
    private bool _unlockThrow = false; //DESBLOQUEO DE HABILIDAD DE LANZAR
    private bool _unlockPullRex = false; //DESBLOQUEO DE HABILIDAD DE LANZAR
    public bool UnlockZipline { get { return _unlockZipline; } set { _unlockZipline = value; } }
    public bool UnlockThrow{ get { return _unlockThrow; } set { _unlockThrow = value; } }
    public bool UnlockPullRex{ get { return _unlockPullRex; } set { _unlockPullRex = value; } }
    public bool IsInControll{ get { return _isInControll; } } 
    public bool CanMove{ get { return _canMove; } } 
    public bool ArmPulled{ get { return _armPulled; } set { _armPulled = value; } } 
    public bool ArmRelease { get { return _armReleased; } set { _armReleased = value; } } 
    public Animator Animator { get { return _animator; } }
    public Rigidbody2D Rigidbody2D{ get { return _rb2D; } } 
    public Vector2 MovementInput { get { return _movementInput; } }
    public Vector2 LastMovementInput { get;  set; } //necesite guardar el ultimo input para la anim del impulse
    public Vector2 LastCardinalInput { get; private set; }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }
    public AudioClip GrabSFX { get { return _grabSFX; } }
    public AudioClip ThrowSFX { get { return _throwSFX; } }
    public AudioClip PullRexSFX { get { return _pullRexSFX; } }
    public AudioClip ZiplineSFX { get { return _ziplineSFX; } }
    public bool IsRecoiling{ get { return _isRecoiling; } set { _isRecoiling = value; } }


    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _armImpulser = GetComponent<ArmImpulser>();
        _auxSpeed = _normalSpeed;
        //este me guarda el valor original al instanciarse, como no esta en update no se actualiza.
        //dsps lo uso para recuperar la velocidad normal el ne fixed
        LastMovementInput = Vector2.down; //inicializo el input en una pos default
    }
    private void FixedUpdate()
    {
        if (!IsInControll || IsRecoiling || !_canMove) return;
        {
            _rb2D.velocity = _movementInput * _normalSpeed;
        }

    }
    public void LowSpeed(bool change)
    {
        if (change)
            _normalSpeed = _lowSpeed;
        else if (!change)
            _normalSpeed = _auxSpeed;


    }
    public void SetMovementInput(Vector2 input)
    {
        if (!IsInControll || !_canMove) return;

        // Guardamos el input real (para movimiento físico y diagonales)
        _movementInput = input;

        // --- Detección de input cardinal dominante ---
        Vector2 cardinalInput = Vector2.zero;

        if (input != Vector2.zero)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                cardinalInput = new Vector2(Mathf.Sign(input.x), 0f);
            else
                cardinalInput = new Vector2(0f, Mathf.Sign(input.y));

            // Guardamos el último input cardinal (para animaciones y disparos)
            LastCardinalInput = cardinalInput;
            LastMovementInput = input.normalized; // si querés guardar también la diagonal real
        }

        // --- Animator ---
        // Usa los valores cardinales para dirección (sin diagonales)
        _animator.SetFloat("Horizontal", LastCardinalInput.x);
        _animator.SetFloat("Vertical", LastCardinalInput.y);

        // Usa la magnitud real para la velocidad (para transiciones suaves)
        _animator.SetFloat("Speed", _movementInput.magnitude);
    }
    public void StopMovement()
    {
        if (_rb2D == null)
        {
            Debug.LogError(gameObject.name + "there is not rigidbody 2d!");
            return;
        }
        _movementInput = Vector2.zero;
        _rb2D.velocity = Vector2.zero;
        _animator.SetFloat("Horizontal", LastMovementInput.x);
        _animator.SetFloat("Vertical", LastMovementInput.y);
        _animator.SetFloat("Speed", 0f);
    }
    public void SwitchArmType(bool type)
    {
        if (!IsInControll) return;
        _armImpulser.SwitchArmType(type);
    }
    public ImpulseType GetCurrentArmType()
    {
        return _armImpulser.CurrentType;
    }

    public void PerformThrowArm(ImpulseType type)
    {
        if (!IsInControll) return;
        _armImpulser.GetThrowArm(type);
    }
    public void PerformArmToAnchor(Transform closestAnchor, bool isHoldingAnchor)
    {
        if (!IsInControll) return;
        _armImpulser.GetArmToAnchor(closestAnchor, isHoldingAnchor);

            //no puede impulsarse a un anclaje si esta sosteniendo un objeto
    }


    public void SetControl(bool isActive)
    {
        _isInControll = isActive;
        if (!isActive) StopMovement();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _canMove = isEnabled;

    }
    private void OnTriggerEnter2D(Collider2D collision) // detecto si esta en zona elevada para jugar ocn las llayers con la que peude chcoar el disparo
    {
        if (collision.CompareTag("HighGround"))
        {
            _isOnHighGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HighGround"))
        {
            _isOnHighGround = false;
        }
    }
    public bool IsOnHighGround()
    {
        return _isOnHighGround;
    }

    public bool GetControl()
    {
        return IsInControll;
    }
}



