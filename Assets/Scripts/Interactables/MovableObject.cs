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
    private Vector2 _velocity = Vector2.zero;
    [SerializeField] private float _stopThreshold = 0.05f;
    [SerializeField] private AudioClip _moovingSFX;

    [Header("Collision")]
    [SerializeField] private LayerMask _obstacleMask;
    private Vector2 _targetPosition;
    private bool _isBeingMoved = false;
    private BoxCollider2D _collider2D;

    private Vector2 _currentPosition;

    private bool _canMove = true;
    private bool _isStopping = false;

    private WeightedObject weighted;

    private void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        weighted = GetComponent<WeightedObject>();
    }

    public void MoveTo(Vector2 position)
    {
        if (_canMove)
        {
            if (weighted)
            {
                float pushFactor = 1f / (1f + weighted.GetWeight() * 0.1f);
                _targetPosition = Vector2.Lerp(transform.position, position, pushFactor);
            }
            else
                _targetPosition = position;

            _canMove = false;
            _isBeingMoved = true;
            SFXManager.Instance.PlaySFX(_moovingSFX);
        }
    }

    void Update()
    {
        if (_isBeingMoved)
        {
            _currentPosition = transform.position;
            Vector2 direction = _targetPosition - _currentPosition;
            float distance = direction.magnitude;

            if (distance > 0f)
            {
                Vector2 dirNormalized = direction / distance;

                // Check for obstacles directly in the path
                RaycastHit2D hit = Physics2D.BoxCast(_currentPosition, _collider2D.size, 0f, dirNormalized, distance, _obstacleMask);

                if (hit.collider != null)
                {
                    // Obstacle detected — stop movement immediately
                    _velocity = Vector2.zero;
                    _canMove = true;
                    _isBeingMoved = false;
                    return; // exit Update to avoid moving into the obstacle
                }
            }

            // Smooth movement
            Vector2 smoothPosition = Vector2.SmoothDamp(_currentPosition, _targetPosition, ref _velocity, _moveSmoothTime);

            if (Vector2.Distance(smoothPosition, _targetPosition) < _stopThreshold)
            {
                _velocity = Vector2.zero;
                _canMove = true;
                _isBeingMoved = false;
            }

            transform.position = smoothPosition;
        }
    }
}
