using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]
public class ChildPlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _climbSpeed;
    private float _currentSpeed; 
    [SerializeField] private AudioClip _climbingLoopSFX; 
    [SerializeField] private AudioClip _footstepsSFX; 
    private Rigidbody2D _rb2D;
    private Collider2D _collider;
    private Animator _animator;
    private Vector2 _movementInput;
    private bool _canMove;

    public bool isInControll = false;
    //Public properties 
    public Animator Animator { get { return _animator; }} 
    public AudioClip ClimbSFX  { get { return _climbingLoopSFX; }} 
    public AudioClip StepsSFX { get { return _footstepsSFX; }} 
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
        _currentSpeed = _speed;
        _collider = GetComponent<Collider2D>();
        ClimbDetector = GetComponentInChildren<ClimbDetector>();
        //Climbdetector in an child object
    }

    public void SetMovementInput(Vector2 input)
    {
        //ask for control first
        if (!isInControll || !_canMove) return;
        {
            _movementInput = input.normalized;
            _animator.SetFloat("Horizontal", _movementInput.x);
            _animator.SetFloat("Vertical", _movementInput.y);
            _animator.SetFloat("Speed", _movementInput.magnitude);

        }
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
        _animator.SetFloat("Speed", 0f);
    }

    private void FixedUpdate()
    {
        if (!isInControll || !_canMove) return;
        _rb2D.velocity = _movementInput * _currentSpeed; 
        //Currentspeed because child have two velocitys, one for climbing and other to move.
    }

    public void SetControl(bool isActive)
    {
        isInControll = isActive;
        if (!isActive) StopMovement();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _canMove = isEnabled;
    }
}