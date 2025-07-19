using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]
public class OldPlayerBehaviour : MonoBehaviour, IControllable
{

    [SerializeField] private float _speed;
    [SerializeField] private AudioClip _footstepsSFX;
    private bool _canMove; //manage from CharacterManager
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private ArmImpulser _armImpulser;
    private Vector2 _movementInput;
    private bool _isInControll;
    private bool _isRecoiling = false;
    public bool IsInControll{ get { return _isInControll; } set { _isInControll = value; } } 
    public Animator Animator { get { return _animator; } } 
    public Vector2 MovementInput { get { return _movementInput; } }
    public AudioClip StepsSFX { get { return _footstepsSFX; } }
    public bool IsRecoiling{ get { return _isRecoiling; } set { _isRecoiling = value; } } 

    //OLD BACKUP---- tengo que hacer la clase base
    //public void PerformThrowArm(ImpulseType type)
    //{
    //    if (!IsInControll) return;
    //    _armImpulser.GetThrowArm(type);
    //}
//END BACKUP ... tengo que hacer la clase base 
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _armImpulser = GetComponent<ArmImpulser>();
    }

    public void SetMovementInput(Vector2 input)
    {
        if (!IsInControll || !_canMove) return; 
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
        if (!IsInControll || IsRecoiling) return;
        {
            _rb2D.velocity = _movementInput * _speed;
        }
    }


    public void PerformThrowArm(ImpulseType type)
    {
        if (!IsInControll) return;
        _armImpulser.GetThrowArm(type);
    }

    public void SetControl(bool isActive)
    {
        IsInControll = isActive;
        if (!isActive) StopMovement();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _canMove = isEnabled;
    }
}



