using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]

public class AgilePlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private Transform _mouth;
    [SerializeField] private float _speed;
    [SerializeField]private float _jumpDelay = 0.5f;
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
    //it remains to be encapsulated
    public bool isInControll = false;
    //timer para cd salto
    private float _jumpTimer = 0;
    private bool _delayCompleted =false;

    //Public properties para acceder desde los estados
    //muchos de ellos necesarios para hacer bien el salto

    public bool DelayCompleted { get { return _delayCompleted; } }
    public bool CanJump { get { return _canJump; } set { _canJump = value; } }
    public bool CanDig { get { return _canDig; } set { _canDig = value; } }
    public Vector2 MovementInput { get { return _movementInput; } }
    public Collider2D PlayerCollider { get { return _collider2D; } set { _collider2D = value; } }
    public Rigidbody2D Rigidbody2D { get { return _rb2D; } set { _rb2D = value; } }
    public HoleDetector HoleDetector { get; private set; }
    public Animator Animator { get { return _animator; } }
    public AudioClip DigSFXClip { get { return _digSFXClip; } }
    public AudioClip JumpSFXClip { get { return _jumpSFXClip; } }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        HoleDetector = GetComponentInChildren<HoleDetector>(); //rever esto, puedo integrarlo en el constructor del estado como el Jump
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
        if (Mathf.Abs(dir.y) > 0.01f)
        {
            mouthPos.y = Mathf.Abs(mouthPos.y) * Mathf.Sign(dir.y);
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

