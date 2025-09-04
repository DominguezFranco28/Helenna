
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class ArmImpulser : MonoBehaviour
{
    //Variables to adjust parameters of the IMPULSE force
    [SerializeField] private float _moveSmoothTime; 
    [SerializeField] private bool _isRecoiling = false;
    [SerializeField] private float _spawnTimer;
    private Vector2 _recoilTarget;
    private Vector2 _recoilVelocity;
    private Rigidbody2D _rb2D;


    //Variables tied to the player's arm:
    [SerializeField] private GameObject _armShot;
    [SerializeField] private ArmLineController _armLinePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private AudioClip _dashSFX;
    [SerializeField] private AudioClip _throwSFX;
    private ArmImpulser _impulser;
    private GameObject _currentArmBullet;
    private ArmLineController _currentArmLine;
    private Collider2D _playerCol;


    //Variables to obtain components external to the arm.
    private OldPlayerBehaviour _movementBehaviour;


    //Public methods so that the methods of this main mechanic (private)
    //can be accessed from the OldPlayerBehaviour int the states machine constructor 
    //and the Armbullet script (which manages the logic of the thrown arm).

    public void MovePlayerToAnchor(Vector2 anchorPosition, ImpulseType type)
    {
        StartCoroutine(ApplyRecoil(anchorPosition, type));
    }
    public void GetThrowArm(ImpulseType type)
    {
        ThrowArm(type);
    }
    public void GetArmToAnchor(Transform closestAnchor , bool IsHoldingAnchor)
    {
        ArmToAnchor(closestAnchor, IsHoldingAnchor);
    }
    void Start()
    {
        //I'll ​​leave the link to the other script established. From here I can use other methods or properties.
        _playerCol = GetComponent<Collider2D>();
        _movementBehaviour = GetComponent<OldPlayerBehaviour>();
        _impulser = this;
        _rb2D = GetComponent<Rigidbody2D>();
 
    }
    private void FixedUpdate()
    {
        if (_isRecoiling)
        {
            float stopThreshold = 0.1f; //ojo con este valor porque si lo subia daba problemas
            float smoothTime = _moveSmoothTime;

            Vector2 currentPosition = _rb2D.position;
            Vector2 newPosition = Vector2.SmoothDamp(currentPosition, _recoilTarget, ref _recoilVelocity, smoothTime);

           
            //buscar forma de detectar colisiones para frenar movimiento 
            
            _rb2D.MovePosition(newPosition); //este metodo mas efgectivo para el uso de fisicas con rb que el SmoothDump q no usa fisicas

            if (Vector2.Distance(newPosition, _recoilTarget) <= stopThreshold)
            {
                _rb2D.MovePosition(_recoilTarget); // Para corregir posicion final exacta
                _isRecoiling = false;

            }
        }
    }


    private IEnumerator ApplyRecoil(Vector2 anchorPosition, ImpulseType type) //pasar a maquina de estados
    {
        //refactorizacion para que use sistemas de fisica (controla el fixed updte del behavour) para evitar bugs
        if (type != ImpulseType.Pull)
            yield break;

        SFXManager.Instance.PlaySFX(_dashSFX);
        _movementBehaviour.IsRecoiling = true;
        _movementBehaviour.SetMovementEnabled(false);
        _playerCol.enabled = false; //vital agregarlo, me soluciono muchos bugs con las colisiones. Solucion sencilla

        _recoilTarget = anchorPosition;
        _recoilVelocity = Vector2.zero;
        _isRecoiling = true;

        // Espera hasta que termine el recoil
        while (_isRecoiling)
            yield return new WaitForFixedUpdate();

        _movementBehaviour.IsRecoiling = false;
        _playerCol.enabled = true;
        _movementBehaviour.SetMovementEnabled(true);

    }
    private void ThrowArm(ImpulseType type)
    {
        if (_currentArmBullet != null)
        {
            return; //only let be one active arm.
        }
        StartCoroutine(SpawnArmBullet(type));
    }
    private void ArmToAnchor(Transform closestAnchor, bool IsHoldingAnchor)
    {
        if (!IsHoldingAnchor)
        {
            if (_currentArmLine != null)
                _currentArmLine.CancelLine(); // destruyo la anterior línea si existe

            _currentArmLine = Instantiate(_armLinePrefab);
            _currentArmLine.AssignTarget(_spawnPoint.position, closestAnchor);
        }
        else
        {
            if (_currentArmLine != null)
            {
                _currentArmLine.CancelLine();
                _currentArmLine = null;
            }
        }
    }
    public IEnumerator SpawnArmBullet(ImpulseType type)
    {     
        Vector2 direction = _movementBehaviour.LastMovementInput; //obtengo el ultimo input de movimiento del jugador para disparar el brazo en esa direccion
        yield return new WaitForSeconds(_spawnTimer);
        //Un timer para retrasar el disparo, asi me da tiempo a que se ejecute la animacion de recoil del brazo
        SFXManager.Instance.PlaySFX(_throwSFX);

        //termine seteando la rotacion en 0 porque ajuste con animaciones direccionales
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject armBullet = GameObject.Instantiate(_armShot, _spawnPoint.position, rotation);


        if (armBullet != null)
        {
            //Save the reference of the current arml
            _currentArmBullet = armBullet;

            //Ignore collisions so the arm doesn't collide with the player
            Collider2D bulletCol = armBullet.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(bulletCol, _playerCol);

            //I pass the parameters to the methods that manage the arm logic
            var armScript = armBullet.GetComponent<ArmBullet>();
            armScript.SetDirection(direction);
            armScript.SetImpulseForce(_impulser);
            armScript.SetImpulseType(type);
            armScript.DetectVerticality(_movementBehaviour.IsOnHighGround()); //paso la altura del jugador al metodo que instancia la bala para que modifique su layer
        }
    }
}



