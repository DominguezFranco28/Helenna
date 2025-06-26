using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]
public class PlayerBehaviour : MonoBehaviour, IControllable
{

    [SerializeField] private float _speed;
    [SerializeField] private AudioClip _footstepsSFX;
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private Vector2 _movementInput;
    private ArmImpulser _armImpulser;
    public bool isInControll = true;
   [SerializeField] private bool _canMove;
    public bool isRecoiling = false; //Bandera utilizada para indicar si va el jugador esta en "retroceso" despues de un impulso.
    public Animator Animator { get { return _animator; } } 
    public Vector2 MovementInput { get { return _movementInput; } }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _armImpulser = GetComponent<ArmImpulser>();

    }

    public void SetMovementInput(Vector2 input)
    {
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

        _movementInput = Vector2.zero;
        _rb2D.velocity = Vector2.zero;
        _animator.SetFloat("Speed", 0f); 
    }
    private void FixedUpdate()
    {
        if (!isInControll || isRecoiling) return;
        {
            _rb2D.velocity = _movementInput * _speed;
        }
    }


    public void PerformThrowArm(ImpulseType type)
    {
        if (!isInControll) return;
        _armImpulser.GetThrowArm(type);
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



