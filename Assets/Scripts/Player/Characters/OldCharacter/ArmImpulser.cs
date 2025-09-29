
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;
using System;

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
    private ImpulseType _curretType;
    private ArmLineController _currentArmLine;
    private Collider2D _playerCol;


    //Variables to obtain components external to the arm.
    private OldPlayerBehaviour _movementBehaviour;
    public ImpulseType CurrentType => _curretType;
    //expongo el current type para poder acceder desde la maquina de estados y cambiar la accion segun el tipo de impulso

    //Public methods so that the methods of this main mechanic (private)
    //can be accessed from the OldPlayerBehaviour int the states machine constructor 
    //and the Armbullet script (which manages the logic of the thrown arm).

    //public void MovePlayerToAnchor(Vector2 anchorPosition, ImpulseType type)
    //{
    //    StartCoroutine(ApplyRecoil(anchorPosition, type));
    //}
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


/*    private IEnumerator ApplyRecoil(Vector2 anchorPosition, ImpulseType type)*/ //pasar a maquina de estados
    //{
        //refactorizacion para que use sistemas de fisica (controla el fixed updte del behavour) para evitar bugs
    //    if (type != ImpulseType.Pull)
    //        yield break;

    //    SFXManager.Instance.PlaySFX(_dashSFX);
    //    _movementBehaviour.IsRecoiling = true;
    //    _movementBehaviour.SetMovementEnabled(false);
    //    _playerCol.enabled = false; //vital agregarlo, me soluciono muchos bugs con las colisiones. Solucion sencilla

    //    _recoilTarget = anchorPosition;
    //    _recoilVelocity = Vector2.zero;
    //    _isRecoiling = true;

    //    // Espera hasta que termine el recoil
    //    while (_isRecoiling)
    //        yield return new WaitForFixedUpdate();

    //    _movementBehaviour.IsRecoiling = false;
    //    _playerCol.enabled = true;
    //    _movementBehaviour.SetMovementEnabled(true);

    //}
    private void ThrowArm(ImpulseType type)
    {
        if (_currentArmBullet != null)
        {
            return; //only let be one active arm.
        }
        _curretType = type;
        StartCoroutine(SpawnArmBullet(_curretType));
    }
    private void ArmToAnchor(Transform closestAnchor, bool IsHoldingAnchor)
    {
        if (!IsHoldingAnchor)
        {
            if (_currentArmLine != null)
                _currentArmLine.CancelLine(); // destruyo la anterior línea si existe

            StartCoroutine(WaitToSpawn(closestAnchor));
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
    public IEnumerator WaitToSpawn(Transform anchor)
    {
        yield return new WaitForSeconds(_spawnTimer);
        _currentArmLine = Instantiate(_armLinePrefab);
        _currentArmLine.AssignTarget(_spawnPoint.position, anchor);
        // aviso al CharacterManager que hay una nueva tirolesa disponible , para que nina pueda buscar la referencia desde ahi
        CharacterManager.Instance.SetActiveZipline(_currentArmLine);
    }
    public IEnumerator SpawnArmBullet(ImpulseType type)
    {
        Vector2 direction = _movementBehaviour.LastMovementInput;
        if (direction == Vector2.zero)
            direction = Vector2.down;

        yield return new WaitForSeconds(_spawnTimer);
        SFXManager.Instance.PlaySFX(_throwSFX);

        // ajuste de spawnpoint en base a la direccion 
        Vector3 spawnOffset = Vector3.zero;

        if (direction.x > 0) // mirando derecha
            spawnOffset = new Vector3(0.5f, 0f, 0f);
        else if (direction.x < 0) // mirando izquierda
            spawnOffset = new Vector3(0f, -0.3f, 0f);
        else if (direction.y > 0) // mirando arriba
            spawnOffset = new Vector3(-0.3f, 0.2f, 0f);
        else if (direction.y < 0) // mirando abajo
            spawnOffset = new Vector3(0.3f, -0.2f, 0f);

        Vector3 finalSpawnPos = transform.position + spawnOffset;

   
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        GameObject armBullet = Instantiate(_armShot, finalSpawnPos, rotation);

        if (armBullet != null)
        {
            _currentArmBullet = armBullet;

            Collider2D bulletCol = armBullet.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(bulletCol, _playerCol);

            var armScript = armBullet.GetComponent<ArmBullet>();
            armScript.SetDirection(direction);
            armScript.SetImpulseType(type);
            //armScript.DetectVerticality(_movementBehaviour.IsOnHighGround());
            armScript.SetStartTransform(_spawnPoint);
        }
    }

    public void SwitchArmType( bool type)
    {
        if (type) { _curretType = ImpulseType.Push; }
        else { _curretType = ImpulseType.Pull; }

        //si estaba en pussh pasa a pull y vicveversa
        Debug.Log("Current Arm Type: " + _curretType);
    }
}



