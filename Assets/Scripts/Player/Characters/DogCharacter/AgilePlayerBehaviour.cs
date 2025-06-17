using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgilePlayerBehaviour : MonoBehaviour, IControllable
{
    [SerializeField] private Transform _mouth;
    [SerializeField] private float _speed;
    [SerializeField] private AudioClip _footstepsSFX;
    [SerializeField] private AudioClip _digSFXClip;
    [SerializeField] private Animator _animator;
    private Rigidbody2D _rb2D;
    private Vector2 _movementInput;
    private Collider2D _collider2D;

    //Queda pendiente encapsular bien estas variables.
    public bool canMove;
    public bool isInControll = false;


    //Exposición de variables con metodos publicos (para manipular desde la statemachine y otros)
    public Animator Animator => _animator;
    public AudioClip DigSFXClip => _digSFXClip;
    public AudioClip StepsSFX { get { return _footstepsSFX; } }

    public Vector2 MovementInput { get { return _movementInput; } }
    public HoleDetector HoleDetector { get; private set; }
    public Collider2D PlayerCollider { get { return _collider2D; } }

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        HoleDetector = GetComponentInChildren<HoleDetector>();
        _collider2D = GetComponent<Collider2D>();


    }

    public void SetMovementInput(Vector2 input)
    {
        if (!isInControll || !canMove) return;
        {
            _movementInput = input.normalized;
            _animator.SetFloat("Horizontal", _movementInput.x);
            _animator.SetFloat("Vertical", _movementInput.y);
            _animator.SetFloat("Speed", _movementInput.magnitude);
            UpdateMouthDirection(_movementInput); //como el pj no rota,
                                                  //+solo cambia su anim, tuve que hacer una rotacion manual a la boca para que funcione la logica de sujetar items
        }
    }
    private void UpdateMouthDirection(Vector2 dir) //Asistido con IA, desconozco mucho de la  utilizacion y metodos con vectores todaiva.
    {
        Vector3 mouthPos = _mouth.localPosition;

        if (Mathf.Abs(dir.x) > 0.01f)
        {
            // Si hay movimiento horizontal, lo seguimos
            mouthPos.x = Mathf.Abs(mouthPos.x) * Mathf.Sign(dir.x);
        }
        else if (dir == Vector2.zero || dir.y > 0.01f)
        {
            // Si no hay input, forzamos que quede a la derecha
            mouthPos.x = Mathf.Abs(mouthPos.x);
        }

        _mouth.localPosition = mouthPos;
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

