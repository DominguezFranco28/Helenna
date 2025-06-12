using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _climbSpeed;
    private float _currentSpeed; //Este fue necesario para establecer una velocidad dinamica, ya que tengo dos tipos de velocidades, con escalada y caminata.
    private Rigidbody2D _rb2D;
    private Collider2D _collider;
    public Animator _animator;
    public SFXPlayerController _sfx;
    public bool canMove;
    public bool isInControll = false;
    private Vector2 _movementInput;

    //prop publicas para referenciar caracteristicas del jugador y poder integrarlo a la logica de estados.

    public Vector2 MovementInput { get { return _movementInput; } }
    public float ClimbSpeed { get { return _climbSpeed; } }
    public Collider2D PlayerCollider { get { return _collider; } }
    public ClimbDetector ClimbDetector { get; private set; } 
    public void SetSpeed(float newSpeed)
    {
        _currentSpeed = newSpeed;
    }
    public float DefaultSpeed
    {
        get { return _speed; }
    }

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sfx = GetComponent<SFXPlayerController>();
        _currentSpeed = _speed;
        _collider = GetComponent<Collider2D>();
        ClimbDetector = GetComponentInChildren<ClimbDetector>(); //lo puse en un childent asi que tengo que tenerlo en cuenta en el getocmponent.
    }

    public void SetMovementInput(Vector2 input)
    {
        if (!isInControll || !canMove) return;
        {
            _movementInput = input.normalized;
            _animator.SetFloat("Horizontal", _movementInput.x);
            _animator.SetFloat("Vertical", _movementInput.y);
            _animator.SetFloat("Speed", _movementInput.magnitude);

        }
    }

    public void StopMovement()
    {
        _movementInput = Vector2.zero;
        _rb2D.velocity = Vector2.zero;
        _animator.SetFloat("Speed", 0f);
    }

    private void FixedUpdate()
    {
        if (!isInControll || !canMove) return;
        _rb2D.velocity = _movementInput * _currentSpeed; //lo cambio al currentspeed para que el fixedupdate trabje tambien respetando la velocidad de escalñada cuando le toque escalar
    }

    public void SetControl(bool isActive)
    {
        isInControll = isActive;
        if (!isActive) StopMovement();
    }
}
