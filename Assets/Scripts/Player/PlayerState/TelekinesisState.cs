using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisState : IState
{
    private PlayerBehaviour _playerMovement;
    private StateMachine _stateMachine;
    private float _duration = 0.5f;
    private float _timer;
    private GameObject _armShot;
    private Transform _spawnPoint;
    private TelekinesisForce _telekinesisForce;

    private Collider2D _armCol;
    private Collider2D _playerCol;
    public TelekinesisState(PlayerBehaviour player, StateMachine stateMachine, GameObject armShot, Transform spawnPoint, TelekinesisForce telekinesisForce)
    {
        _playerMovement = player;
        _stateMachine = stateMachine;
        _armShot = armShot;
        _spawnPoint = spawnPoint;
        _telekinesisForce = telekinesisForce;
    }
    public void Enter()
    {
        //Animacion de uso telekinetico. Tal vez una especie de explosion de vapor desde el brazo?
        //_playerMovement.canMove = false;
        Debug.Log("Entraste al estado: TELEKINESIS");
        _timer = _duration;
        _playerMovement.canMove = false;
        _playerMovement.StopMovement();
        _playerMovement._animator.SetTrigger("IsTelekinesis");
        _playerCol = _playerMovement.GetComponent<Collider2D>();
    }

    public void Exit()
    {
        //Animacion de salida telekinetico. Tal vez una especie de explosion de vapor desde el brazo, o retraccion del mismo
        //_playerMovement.canMove = true;
        Debug.Log("Saliste del estado: TELEKINESIS");
        _playerMovement.canMove = true;
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            _playerMovement.PerformPush();
            _stateMachine.TransitionTo(_stateMachine.idleState);
        }

        if (Input.GetMouseButtonDown(1))
        {
            _playerMovement.PerformPull();
            _stateMachine.TransitionTo(_stateMachine.idleState);
        }
        //if (_timer <= 0f)
        //{
        //    _stateMachine.TransitionTo(_stateMachine.idleState);
        //}
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _stateMachine.TransitionTo(_stateMachine.idleState); //Pasaje a estado de idle
            _playerMovement.canMove = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowArm();
        }
        //Debug.Log(_playerMovement.canMove);
    }
    private void ThrowArm() //Probar con un input.
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)_spawnPoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Rotacion del proyectil apra que mire a donde apunta el mouse
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject bullet = GameObject.Instantiate(_armShot, _spawnPoint.position, rotation);

        //Ignorar colisiones para qeu el brazo no choque con el jugador
        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bulletCol, _playerCol);


        var armScript = bullet.GetComponent<ArmBullet>();
        armScript.SetDirection(direction);
        armScript.SetTelekinesisForce(_telekinesisForce); // Pasaje de la referencia al telekinesis force para aplicar el Recoil.
    }
}
