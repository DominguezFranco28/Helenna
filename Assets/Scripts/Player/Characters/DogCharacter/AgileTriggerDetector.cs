using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AgileTriggerDetector : MonoBehaviour
{
    [SerializeField] private AgilePlayerBehaviour _playerBehaviour;
    [SerializeField] private AgilePlayerController _playerController;
    [SerializeField] private LayerMask _waterLayer;
    [SerializeField] private float _distance = 3f;
    [SerializeField] private Vector2 _origin;
    [SerializeField] private Vector2 _boxcastSize;
    private AgileStateMachine _stateMachine;
    [SerializeField] private string _waterLayerName = "Water";
    private int _waterLayerIndex; //lo del indicie en la layer necesario para el IgnoreLayerCollision 
    public bool IsInWater { get; private set; } //esto para usar el el throw state y detecte si esta en el agua o no
    public bool WaterAhead { get; private set; } //esto para usar el el throw state y detecte si esta en el agua o no
    public bool IsBeeingPulled{ get; set; }

    private void Awake()
    {
        if (_playerController != null)
        {
            _stateMachine = _playerController.StateMachine; //capto la misma referencia de la maquina de estados, no la inicio de nuevo
        }
        _waterLayerIndex = LayerMask.NameToLayer(_waterLayerName);
    }
    private void Update()
    {
        int mask = LayerMask.GetMask(_waterLayerName);
        _origin = gameObject.transform.position;
        Vector2 direction = _playerBehaviour.PendingThrowDirection; //misma prop que le da direccion de impuslo nque sea la que castee raycast
        if (direction != Vector2.zero)  //validacion para que no dibuje la linea del raycast si no hay input, daba bugs
        {
            RaycastHit2D hit = Physics2D.BoxCast(_origin, _boxcastSize, 0f, direction, _distance, mask);
            Debug.DrawLine(_origin, _origin + direction * _distance, Color.red);

            if (hit.collider != null)
            {
                //tiene agua al frente, sigo desplazando
                Debug.Log("Rex sees water ahead" + IsInWater);
                WaterAhead = true;

            }
            else
            {
                WaterAhead = false;
            }
        }
            Collider2D overlap = Physics2D.OverlapBox(_origin, _boxcastSize, 0f, mask); //calcula pos actual de rex, funciona con tilemap collider solido, el triggerstay no
            IsInWater = overlap != null;
        Debug.Log("Rex is in water: " + IsInWater + " Water ahead: " + WaterAhead);
    }
    public void IgnoreWater(bool enable)
    {

            Physics2D.IgnoreLayerCollision(_playerBehaviour.gameObject.layer, _waterLayerIndex, enable);
            Debug.Log("Ignoring water collision: " + enable);
        //ignorar colision con el auga

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //I turn off the fence collider when the dog detects the hole
        if (collision.CompareTag("Hole"))
        {
            Transform parent = collision.transform.parent;
            if (parent != null)
            {
                Collider2D parentCollider = parent.GetComponent<Collider2D>();
                if (parentCollider != null)
                {
                    parentCollider.enabled = false;
                }
            }
            _playerBehaviour.CanDig = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Hole"))
        {
            Transform parent = collision.transform.parent;
            if (parent != null)
            {
                Collider2D parentCollider = parent.GetComponent<Collider2D>();
                if (parentCollider != null)
                {
                    parentCollider.enabled = true;
                }
            }
            _playerBehaviour.CanDig = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_playerBehaviour == null) return;
        if (_playerBehaviour.PendingThrowDirection == Vector2.zero) return;

        Vector2 direction = _playerBehaviour.PendingThrowDirection.normalized;

        // Tamaño de la caja
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Posición inicial (centro del objeto)
        Vector2 startCenter = (Vector2)transform.position;

        // Posición final (desplazada por la distancia del cast)
        Vector2 endCenter = startCenter + direction * _distance;

        // Color para inicio y fin
        Gizmos.color = Color.cyan;

        // Dibujo caja inicial
        Matrix4x4 startMatrix = Matrix4x4.TRS(startCenter, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.matrix = startMatrix;
        Gizmos.DrawWireCube(Vector3.zero, _boxcastSize);

        // Dibujo caja final
        Matrix4x4 endMatrix = Matrix4x4.TRS(endCenter, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.matrix = endMatrix;
        Gizmos.DrawWireCube(Vector3.zero, _boxcastSize);

        // Reset matrix
        Gizmos.matrix = Matrix4x4.identity;

        // Línea entre las dos cajas para referencia
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startCenter, endCenter);
    }

}





