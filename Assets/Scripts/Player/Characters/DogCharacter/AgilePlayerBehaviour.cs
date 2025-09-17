using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]

public class AgilePlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private Transform _mouth;
    private Vector2 _mouthOriginalPos;

    [SerializeField] private float _speed;
    [SerializeField]private float _jumpDelay = 0.2f;
    [SerializeField] private AudioClip _footstepsSFX;
    [SerializeField] private AudioClip _digSFXClip;
    [SerializeField] private AudioClip _jumpSFXClip;
    private Animator _animator;
    private Rigidbody2D _rb2D;
    private Collider2D _collider2D;
    private Vector2 _movementInput;
    private bool _canMove;
    private bool _canJump = false;
    private bool _canDig = false;
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
    public bool CanDig { get { return _canDig; } set { _canDig = value; } }
    public Vector2 MovementInput { get { return _movementInput; } }
    public Vector2 LastMovementInput { get; set; }
    public Collider2D PlayerCollider { get { return _collider2D; } set { _collider2D = value; } }
    public Rigidbody2D Rigidbody2D { get { return _rb2D; } set { _rb2D = value; } }
    public HoleDetector HoleDetector { get; private set; }
    public Animator Animator { get { return _animator; } }
    public AudioClip DigSFXClip { get { return _digSFXClip; } }
    public AudioClip JumpSFXClip { get { return _jumpSFXClip; } }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }

    void Awake()
    {
        Debug.Log("Z ANTES DEL NORMALIZE: " + transform.position.z);
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        HoleDetector = GetComponentInChildren<HoleDetector>(); //rever esto, puedo integrarlo en el constructor del estado como el Jump
        _collider2D = GetComponent<Collider2D>();
        _mouthOriginalPos = _mouth.position;
        NormalizeZ(transform);
    }
 

    public void SetMovementInput(Vector2 input)
    {
        if (!IsInControll || !_canMove) return;
        {

            _movementInput = input.normalized;
            if (_movementInput.magnitude > 0.01f) // aca guardo el ulktimo input para anim de impulse
                LastMovementInput = _movementInput;
            _animator.SetFloat("Horizontal", _movementInput.x);
            _animator.SetFloat("Vertical", _movementInput.y);
            _animator.SetFloat("Speed", _movementInput.magnitude);
            NormalizeZ(transform);
            UpdateMouthDirection(_movementInput); 
            if (_delayCompleted) //revisar esto
            {
                _animator.SetBool("GoIdle", true);
            }
            else
            {
                _animator.SetBool("GoIdle", false);
            }
        }
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
    public void RestartCooldown() //cd para salto. Lo llamo en cada entrada del jumpState
    {
        _jumpTimer = 0;
        _delayCompleted = false;
    }
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

    public void SetControl(bool isActive)
    {
        _isInControll = isActive;
        if (!isActive) StopMovement();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _canMove = isEnabled;
    }

    public  void NormalizeZ(Transform trans, float z = 0f) //metood apra normalizar z en todos los hijos del perro, me daban bugs algunos con la boca
    {
        Vector3 pos = trans.position;
        pos.z = z;
        trans.position = pos;
        foreach (Transform child in trans)
        {
            NormalizeZ(child, z);
        }
    }

    public bool GetControl()
    {
        return IsInControll;
    }
}

