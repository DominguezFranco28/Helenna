using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]

public class AgilePlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private Transform _mouth;
    [SerializeField] private Transform _triggerDetector;
    private Vector2 _mouthOriginalPos;

    [SerializeField] private float _speed;
    [SerializeField]private float _jumpDelay = 0.2f;
    [SerializeField] private AudioClip _footstepsSFX;
    [SerializeField] private AudioClip _digSFXClip;
    [SerializeField] private AudioClip _jumpSFXClip;
    private Animator _animator;
    private Rigidbody2D _rb2D;
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _movementInput;
    private bool _canMove;
    private bool _canJump = false;
    private bool _isInControll = false;
    //timer para cd salto
    private float _jumpTimer = 0;
    private bool _delayCompleted =false;
    private bool _isGrounded;
    //Public properties para acceder desde los estados
    //muchos de ellos necesarios para hacer bien el salto
  
    public bool DelayCompleted { get { return _delayCompleted; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsInControll { get { return _isInControll; } }
    public bool CanJump { get { return _canJump; } set { _canJump = value; } }
    public Vector2 MovementInput { get { return _movementInput; } }
    public Collider2D PlayerCollider { get { return _collider2D; } set { _collider2D = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } set { _spriteRenderer = value; } }
    public Rigidbody2D Rigidbody2D { get { return _rb2D; } set { _rb2D = value; } }
    public AgileTriggerDetector TriggerDetector { get; private set; }
    public Animator Animator { get { return _animator; } }
    public AudioClip DigSFXClip { get { return _digSFXClip; } }
    public AudioClip JumpSFXClip { get { return _jumpSFXClip; } }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }

    public Vector2 LastMovementInput { get; set; }
    public Vector2 LastCardinalInput { get; private set; }
    public Vector2 PendingThrowDirection { get; set; } //direccion que sera obtenida cuando harold lo lance
    public Vector2 PendingPulledDirection { get; set; } //direccion que sera obtenida cuando harold lo atraiga
    public RexTPHole CurrentHole { get; private set; }

    public void SetCurrentHole(RexTPHole hole)
    {
        CurrentHole = hole;
    }

    public void ClearCurrentHole()
    {
        CurrentHole = null;
    }
    void Awake()
    {
        Debug.Log("Z ANTES DEL NORMALIZE: " + transform.position.z);
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        TriggerDetector = GetComponentInChildren<AgileTriggerDetector>(); //rever esto, puedo integrarlo en el constructor del estado como el Jump
        _collider2D = GetComponent<Collider2D>();
        _mouthOriginalPos = _mouth.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        NormalizeZ(transform);
    }
    //PARAMETROS DE CONTROL Y MOVIMIENTO
    public void SetControl(bool isActive)
    {
        _isInControll = isActive;
        if (!isActive) StopMovement();
    }
    public bool GetControl()
    {
        return IsInControll;
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _canMove = isEnabled;
    }
 

    public void SetMovementInput(Vector2 input)
    {
        //ask for control first
        if (!IsInControll || !_canMove) return;
        _movementInput = input;

        //input cardinal dominante
        Vector2 cardinalInput = Vector2.zero;

        if (input != Vector2.zero)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                cardinalInput = new Vector2(Mathf.Sign(input.x), 0f);
            else
                cardinalInput = new Vector2(0f, Mathf.Sign(input.y));

            // Guardamos el último input cardinal (para animaciones y disparos)
            LastCardinalInput = cardinalInput;
            LastMovementInput = input.normalized; // diaognal real
        }

        if (_animator)
        {
            // --- Animator ---
            // Usa los valores cardinales para direccion
            _animator.SetFloat("Horizontal", LastCardinalInput.x);
            _animator.SetFloat("Vertical", LastCardinalInput.y);

            // Usa la magnitud real para la velocidad (para transiciones suaves)
            _animator.SetFloat("Speed", _movementInput.magnitude);
        }
            UpdateMouthDirection(_movementInput); 
            //if (_delayCompleted) //revisar esto //;LOGICA VIEJA DE SALTO EN PIEDRAS
            //{
            //    _animator.SetBool("GoIdle", true);
            //}
            //else
            //{
            //    _animator.SetBool("GoIdle", false);
            //}
    }
    private void UpdateMouthDirection(Vector2 dir) 
    {
        if (dir == Vector2.zero)
        {
            _mouth.position = _mouth.transform.position;
             return;
        }
        
        //revbisar esto, lo saque con ia xq desconozco el funcionamiento de estos metodos

        // Calcula el ángulo en radianes y lo convierte a grados
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Aplica la rotación al objeto de la boca
       // _mouth.rotation = Quaternion.Euler(0, 0, angle);

        // Detecta si el input es principalmente vertical
        bool isVertical = Mathf.Abs(dir.y) > Mathf.Abs(dir.x);

        // Consigue el collider del personaje (no de la boca)
        Collider2D parentCol = _mouth.transform.parent.GetComponent<Collider2D>();

        // Calcula el punto en el borde
        Vector2 targetPoint = parentCol.ClosestPoint(parentCol.transform.position + (Vector3)dir.normalized * 10f);

        // Si el movimiento es vertical, ajusta la posición más centrada y hacia afuera
        if (isVertical)
        {
            // Desplaza la boca un poco más afuera del borde en la dirección vertical
            targetPoint += dir.normalized * 0.2f; // Prueba con valores como 0.2f, ajusta según el tamaño
        }
        else
        {
            // En horizontal, puedes ajustar menos o mantener el borde
            targetPoint += dir.normalized * 0.1f;
        }

        _mouth.position = targetPoint;
    
}
    

    public void StopMovement()
    {
        if (_rb2D == null)
        {
            Debug.Log(gameObject.name + "there is no rigidbody 2d for agileCharacter");
            return;
        }

        _movementInput = Vector2.zero;
        _rb2D.velocity = Vector2.zero;
        _animator.SetFloat("Speed", 0f);
    }

    private void FixedUpdate()
    {
        if (!IsInControll || !_canMove) return;
        _rb2D.velocity = _movementInput * _speed;
        CheckGround();
        UpdateMouthDirection(_movementInput); // Actualiza la dirección de la boca en cada FixedUpdate

        _jumpTimer += Time.deltaTime;
        if (_jumpTimer >= _jumpDelay)
        {
            _delayCompleted = true;
           
            
        }
    }
    private void LateUpdate()
    {
        // fuerzo al ciruclo trigger a estar siempre en la posicion del perro, q se me rompia con los hole o el agua
        //buscarle la vuelt a al boca x aca tambien
        _triggerDetector.localPosition = Vector3.zero;
        NormalizeZ(gameObject.transform);
        NormalizeZ(_triggerDetector);
        NormalizeZ(_mouth); //mantengo la z original de la boca para que no me de problemas con la animacion de esta

    }
    //public void RestartCooldown() //cd para salto. Lo llamo en cada entrada del jumpState
    //{
    //    _jumpTimer = 0;
    //    _delayCompleted = false;
    //}
    public void CheckGround()
    {
        if (_collider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _isGrounded = true;
            _animator.SetBool("IsGrounded", true);

        }
        else
        {
            _isGrounded = false;
            _animator.SetBool("IsGrounded", false);
        }

    }



    public void NormalizeZ(Transform trans, float z = 0f) //metood apra normalizar z en todos los hijos del perro, me daban bugs algunos con la boca
    {
        Vector3 pos = trans.position;
        pos.z = z;
        trans.position = pos;
        foreach (Transform child in trans)
        {
            NormalizeZ(child, z);
        }
    }




}

