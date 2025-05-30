using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgilePlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb2D;
    public Animator _animator;
    public bool canMove;
    public bool isInControll = false;
    private Vector2 _movementInput;

    public Vector2 MovementInput => _movementInput;

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }

    public void SetMovementInput(Vector2 input)
    {
        if (!isInControll || !canMove) return;
        {
            _movementInput = input.normalized;
            //_animator.SetFloat("Horizontal", _movementInput.x);
            //_animator.SetFloat("Vertical", _movementInput.y);
            //_animator.SetFloat("Speed", _movementInput.magnitude);
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
        _rb2D.velocity = _movementInput * _speed;
    }

    public void SetControl(bool isActive)
    {
        isInControll = isActive;
        if (!isActive) StopMovement();
    }
}
