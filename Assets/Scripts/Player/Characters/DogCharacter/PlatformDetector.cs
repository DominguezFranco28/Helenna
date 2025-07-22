using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    [SerializeField] private GameObject _mouth;
    [SerializeField] private LayerMask _layer;
    [SerializeField] float _distance = 3f;
    private Vector2 _direction;
    private Vector2 _origin;
    private bool _canJump = false;
    private Vector2 _platformPosition;
    public bool CanJump { get { return _canJump; } set { _canJump = value; } }
    public Vector2 PlatFormPosition { get { return _platformPosition; } set { _platformPosition = value; } }

    private void Start()
    {

    }

    private void Update()
    {
        _origin = _mouth.gameObject.transform.position;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector2(horizontal, vertical);

        if (_direction != Vector2.zero) //validacion para que no dibuje la linea del raycast si no hay input, daba bugs
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)_origin, _direction, _distance, _layer);
            Debug.DrawLine(_origin, _origin + _direction * _distance, Color.red);

            if (hit.collider != null)
            {
                CanJump = true;
                //Debug.Log("Detecte una plataforma de salto");
                PlatFormPosition = hit.collider.transform.position;
            }
            else
            {
                CanJump = false;
            }
        }

        else
        {
            CanJump = false;
        }
    }
}
