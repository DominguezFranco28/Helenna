using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(BoxCollider2D))]
public class MovableObject : MonoBehaviour, IMovable
{
    [Header("Soft Move")]
    [SerializeField] private float _moveSmoothTime = 0.2f;
    [SerializeField] private float _stopThreshold = 0.05f;

    [Header("Collision")]
    [SerializeField] private LayerMask _obstacleMask; // the layers with which the object has to collide
    private Vector2 _targetPosition;
    private Vector2 _velocity = Vector2.zero;
    private bool _isBeingMoved = false;
    private BoxCollider2D _collider2D;

    private Vector2 _currentPosition;

    private bool _canMove = true;
    private bool _isStopping = false;
    private void Start()
    {
        this._collider2D = GetComponent<BoxCollider2D>();

        
    }
    public void MoveTo(Vector2 position) // parameter comes from ArmBullet
    {
        if (_canMove)
        {
            _targetPosition = position;
            _isBeingMoved = true;
        }
    }


    void Update()
    {
        //revisar y ajustar
        if (!_isBeingMoved) return;

        _currentPosition = transform.position;
        Vector2 direction = _targetPosition - _currentPosition;
        float distance = direction.magnitude;

        if (!_isStopping) //solo check de colision si no esta frenada la caja
        {
            RaycastHit2D hit = Physics2D.BoxCast(_currentPosition, _collider2D.bounds.size, 0, direction.normalized, distance, _obstacleMask);


            if (hit.collider != null)
            {
                float moveDistance = hit.distance - 0.05f; //para que no se pegue a la colision 
                if (moveDistance < 0) moveDistance = 0f;
                Vector2 collisionPoint = _currentPosition + direction.normalized * moveDistance;
                _targetPosition = collisionPoint;
            }
        }

        //Suavizado del desplazamiento normal de la caja

        Vector2 smoothPosition = Vector2.SmoothDamp(_currentPosition, _targetPosition, ref _velocity, _moveSmoothTime);
        transform.position = smoothPosition;

        if (Vector2.Distance(smoothPosition, _targetPosition) < _stopThreshold) //el threshold editable desde inspector para ajustar a gusto
        {
            if (_isStopping)
            {
                // Solo se detienne completamente si es frenada suave
                _isBeingMoved = false;
                _velocity = Vector2.zero;
                _canMove = false;
                _isStopping = false;
            }
        }
    }
    public void StopMove (Vector2 target) //parametro viene del transform position de la placa de presion o quien frene el mov
   
    {
        _targetPosition = target;
        _isBeingMoved = true; // Habilita el suavizado
        _canMove = false;
        _isStopping = true;

    }
}
