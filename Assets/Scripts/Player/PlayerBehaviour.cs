using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //[SerializeField] private float _speed;
    //private Rigidbody2D _rb2D;
    //private Animator _animator;
    //private TelekinesisPush _telekinesisPush; //Modificar nombre del script //probar ambos scripts

    //private Vector2 _movementInput;
    //public bool canMove = true;

    //private float _cooldownTime;
    //private float _cooldownTimer = 0f;   
    //void Start()
    //{
    //    _rb2D = GetComponent<Rigidbody2D>();
    //    _animator = GetComponent<Animator>();
    //    _telekinesisPush = GetComponent<TelekinesisPush>();
        
    //}

    //private void Update() 
    //{
    //    CheckImputs();
    //    CoolDown();

    //    //GetAxisRaw devuelve valores inmediatos, me sirve mas que el GetAxis por el tema del blend para las animaciones, ademas de generar un movimiento mas deseado.
    //    _movementInput.x = Input.GetAxisRaw("Horizontal");
    //    _movementInput.y = Input.GetAxisRaw("Vertical");
    //    _movementInput = _movementInput.normalized; //Normalizar los imputs para que no hay duplicacion en el valor de movimiento con las diagonales


    //    _animator.SetFloat("Horizontal", _movementInput.x);
    //    _animator.SetFloat("Vertical", _movementInput.y);
    //    _animator.SetFloat("Speed", _movementInput.magnitude);

    //}
    //private void FixedUpdate()
    //{
    //    //En FixedUpdate porque se trrabaja sobre el RB
    //    _rb2D.velocity = _movementInput * _speed;
    //}
    //private void CoolDown()
    //{

    //    if (_cooldownTimer > 0)
    //    {
    //        _cooldownTimer -= Time.deltaTime;
    //    }
    //}

    //private void CheckImputs()
    //{
    //    if (canMove) //Bandera manejada por el Script que contenga el recoil, para frenar el movimiento del jugador cuando este este realizando X accion. Seguro termine modificando si aplico states.
    //    {   
    //        if (Input.GetMouseButtonDown(0) && _cooldownTimer <= 0f || Input.GetKeyDown(KeyCode.Q) && _cooldownTimer <= 0f) 
    //        {
    //            _telekinesisPush.MouseGet();
    //            _cooldownTimer = _cooldownTime; // Reiniciamos el cooldown
                
    //        }
    //        if (Input.GetMouseButtonDown(1) && _cooldownTimer <= 0f || Input.GetKeyDown(KeyCode.E) && _cooldownTimer <= 0f) 
    //        {
    //            _telekinesisPush.MouseGetPull();
    //            _cooldownTimer = _cooldownTime;
    //        }
    //        //IMPLEMENTAR DASH
    //        if (Input.GetKeyDown(KeyCode.Space))  //Al hacer la variable isGrounded static en el script que chequea la colision del piso, puedo llamarla asi
    //        {
    //            //...
    //        }

    //    }
    //}
    
    [SerializeField] private float _speed;
    private Rigidbody2D _rb2D;
    public Animator _animator;
    private ArmImpulser _armImpulser;
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
        if (canMove)
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
        if (!isRecoiling)
        {
            _rb2D.velocity = _movementInput * _speed;
        }
    }


    public void PerformThrowArm(ImpulseType type)
    {
        _armImpulser.GetThrowArm(type);
    }

}



