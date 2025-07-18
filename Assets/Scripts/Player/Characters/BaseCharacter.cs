using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{

    //revisar y convertir en abstracta
    [SerializeField] private protected float _speed;
    [SerializeField] private AudioClip _footstepsSFX;
    private bool _canMove; //manage from CharacterManager
    private protected Rigidbody2D _rb2D;
    private Animator _animator;
    private protected Vector2 _movementInput;
    private bool _isInControll;
    public bool IsInControll { get { return _isInControll; } set { _isInControll = value; } }
    //Public properties
    public Animator Animator { get { return _animator; } }
    public Vector2 MovementInput { get { return _movementInput; } }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (!_isInControll) return;
        {
            _rb2D.velocity = _movementInput * _speed;
        }
    }
    public void SetMovementInput(Vector2 input)
    {
        if (!_isInControll || !_canMove) return;
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
    public void SetControl(bool isActive)
    {
        _isInControll = isActive;
        if (!isActive) StopMovement();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _canMove = isEnabled;
    }
}

