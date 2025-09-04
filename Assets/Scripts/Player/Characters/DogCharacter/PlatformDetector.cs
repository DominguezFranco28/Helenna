using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    [SerializeField] private GameObject _mouth;
    [SerializeField] private LayerMask _layer;
    [SerializeField] float _distance = 3f;
    private AgilePlayerBehaviour _playerBehaviour;
    private Vector2 _direction;
    private Vector2 _origin;

    private Vector2 _platformPosition;
    private Collider2D _lastPlatform;
    public Vector2 PlatFormPosition { get { return _platformPosition; } set { _platformPosition = value; } }

    private void OnMove(Vector2 movement)
    {
        Debug.Log("dog - OnMove");
        _direction = movement;
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.Move += OnMove;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.Move -= OnMove;
    }

    private void Start()
    {
        _playerBehaviour = GetComponent<AgilePlayerBehaviour>();
    }

    private void Update()
    {
        if (!_playerBehaviour.isInControll)
            return;
        _origin = _mouth.gameObject.transform.position;

        if (_direction != Vector2.zero) //validacion para que no dibuje la linea del raycast si no hay input, daba bugs
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)_origin, _direction, _distance, _layer);
            Debug.DrawLine(_origin, _origin + _direction * _distance, Color.red);

            if (hit.collider != null)
            {
                // Si es una plataforma nueva
                if (hit.collider != _lastPlatform)
                {
                    _playerBehaviour.CanJump = true;
                    PlatFormPosition = hit.collider.transform.position;
                    _lastPlatform = hit.collider;
                 //aviso que se detecto nueva plataforma
                }
                else
                {
                    // Ya esta en esta plataforma
                }
            }
            else
            {
                _playerBehaviour.CanJump = false;
                PlatFormPosition = Vector2.zero;
                _lastPlatform = null; // Ya no estás tocando ninguna plataforma
            }
        }

    }
}
