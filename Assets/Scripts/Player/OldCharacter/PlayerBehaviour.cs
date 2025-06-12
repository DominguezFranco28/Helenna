using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IControllable
{
    
    [SerializeField] private float _speed;
    private Rigidbody2D _rb2D;
    public Animator _animator;
    private ArmImpulser _armImpulser;
    public bool isInControll = true;
    public bool canMove;
    public bool isRecoiling = false; //Bandera utilizada para indicar si va el jugador esta en "retroceso" despues de un impulso.
    private Vector2 _movementInput;

    public Vector2 MovementInput { get { return _movementInput; } }

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _armImpulser = GetComponent<ArmImpulser>();

    }

    public void SetMovementInput(Vector2 input)
    {
        if (!isInControll || !canMove) return; //Con esto me aseguro de no manipular al personjae cuando hago el cambio desde el CharacterManager
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
        _animator.SetFloat("Speed", 0f); // tal vez modiofique con otra animaicon.
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
}



