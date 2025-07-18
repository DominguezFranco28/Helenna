using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]

public class AgilePlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private Transform _mouth;
    [SerializeField] private float _speed;
    [SerializeField] private AudioClip _footstepsSFX;
    [SerializeField] private AudioClip _digSFXClip;
    private Animator _animator;
    private Rigidbody2D _rb2D;
    private Collider2D _collider2D;
    private Vector2 _movementInput;
    private bool _canMove;

    //it remains to be encapsulated
    public bool isInControll = false;


    //Public properties
    public Vector2 MovementInput { get { return _movementInput; } }
    public Collider2D PlayerCollider { get { return _collider2D; } }
    public HoleDetector HoleDetector { get; private set; }
    public Animator Animator { get { return _animator; } }
    public AudioClip DigSFXClip { get { return _digSFXClip; } }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        HoleDetector = GetComponentInChildren<HoleDetector>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void SetMovementInput(Vector2 input)
    {
        if (!isInControll || !_canMove) return;
        {
            _movementInput = input.normalized;
            _animator.SetFloat("Horizontal", _movementInput.x);
            _animator.SetFloat("Vertical", _movementInput.y);
            _animator.SetFloat("Speed", _movementInput.magnitude);
            UpdateMouthDirection(_movementInput); 
        }
    }
    private void UpdateMouthDirection(Vector2 dir) 
    {
        Vector3 mouthPos = _mouth.localPosition;

        if (Mathf.Abs(dir.x) > 0.01f)
        {
            // if there is horizontal movemente, we follow it
            mouthPos.x = Mathf.Abs(mouthPos.x) * Mathf.Sign(dir.x);
        }
        else if (dir == Vector2.zero || dir.y > 0.01f)
        {
            //if there is no input, force to stay at right
            mouthPos.x = Mathf.Abs(mouthPos.x);
        }

        _mouth.localPosition = mouthPos;
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
        _rb2D.velocity = _movementInput * _speed;
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

