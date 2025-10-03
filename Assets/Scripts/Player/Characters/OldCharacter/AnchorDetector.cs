using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnchorDetector : MonoBehaviour
{
    [SerializeField] private float _lockOnDistance = 5f; // largo del BoxCast
    [SerializeField] private float _boxWidth = 1f;       // ancho del BoxCast
    [SerializeField] private LayerMask _anchorLayer;
    [SerializeField] private GameObject _UI;
    [SerializeField] private CinemachineVirtualCamera _cameraFollow;
    private ArmImpulser _armImpulser;
    private OldPlayerBehaviour _oldPlayerBehaviour;
    private Transform _closestAnchor = null;
    private Vector2 _defaultAnchorDirection = Vector2.right; // dirección por defecto si no hay input

    public Transform ClosestAnchor { get { return _closestAnchor; } }
    public AnchorType CurrentAnchor { get; private set; }

    //propr publicty para retornar el tipo de anclaje del punto de anclaje seleccionado

    private void Start()
    {
        _armImpulser = GetComponent<ArmImpulser>();
        _oldPlayerBehaviour = GetComponent<OldPlayerBehaviour>();
    }
    private void Update()
    {
        if (!_oldPlayerBehaviour.IsInControll)
            SetAnchorUI(false); //si no estoy en control apago todo

        Transform detectedAnchor = DetectClosestAnchor();

        // reaccion solo al cambio de anclaje para no est aupdateando siempre la localizaicon
        if (detectedAnchor != _closestAnchor)
        {
            HandleAnchorChanged(detectedAnchor);
        }
    }
    private void HandleAnchorChanged(Transform newAnchor)
    {
        _closestAnchor = newAnchor;

        if (_closestAnchor != null)
        {
            LockOnToAnchor(_closestAnchor);
            SetAnchorUI(true);
        }
        else
        {
            SetAnchorUI(false);
        }
    }
  
    private void SetAnchorUI(bool state) //logica para activa ui y camara
    {
        if (_UI != null && _UI.activeSelf != state)
            _UI.SetActive(state);

        if (_cameraFollow != null)
        {
            if (_cameraFollow.gameObject.activeSelf != state)
                _cameraFollow.gameObject.SetActive(state);

            _cameraFollow.Follow = state ? _closestAnchor : null; //operador ternario (?), si state es true sigue al anchor, si no no sigue a nada
        }
    }
    public Transform DetectClosestAnchor() //metodo para detectar los puntos de anclaje cercano, retorna el transform del punto de anclaje mas cercano
    {
        Vector2 direction = _oldPlayerBehaviour.LastMovementInput;
        // Si no hay input, usamos la última dirección válida
        if (direction.magnitude < 0.01f)
        {
            direction = _defaultAnchorDirection;
        }
        else
        {
            // actualizamos la dirección por defecto solo cuando hay input
            _defaultAnchorDirection = direction.normalized;
        }
        // tamano de caja y angulo
        Vector2 size = new Vector2(_lockOnDistance, _boxWidth);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,  // centro
            size,                // tamaño
            angle,               // rotacion
            direction,           // direccion
            _lockOnDistance,     // distancia
            _anchorLayer         // capa de anclajes que quiero detectar
        );

        if (hit.collider != null) //si toca con un anclaje, devuelvo su transform
        {
            return hit.transform;
        }

        return null;
    }
    public void LockOnToAnchor(Transform anchor) //enfoca al punto de anclaje retornado
    {
        if (anchor == null) return;   
       // if (!_oldPlayerBehaviour.UnlockZipline) return; //si no tengo desbloqueada la habilidad de anclaje no hago nada

        switch (anchor.tag) //establezco el tipo de anclaje segun el tag del objeto
        //        Debug.Log("Locking on to anchor: " + anchor.name);
        {
            case "Zipline":
                CurrentAnchor = AnchorType.Zypline;
                break;
            case "Pipe":
                CurrentAnchor = AnchorType.HookPipe;
                break;
            case "HookPoint":
                CurrentAnchor = AnchorType.HookPoint;
                break;
            // Puedes agregar más casos
            default:
                CurrentAnchor = AnchorType.None;
                break; //seguramente sig acreciendo por eso hice switch.
        }
        //la logica segun el tipo de anclaje va a dirigirla el state machine del player

        //AGREGAR bien la REF A UI para los pop ups

    }
    void OnDrawGizmosSelected()
    {
        //todo ia papa este gizmo
        if (_oldPlayerBehaviour == null) return;

       // if (_oldPlayerBehaviour.LastMovementInput == Vector2.zero) return;

        Vector2 direction = _oldPlayerBehaviour.LastMovementInput;
        Vector2 size = new Vector2(_lockOnDistance, _boxWidth);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector2 castCenter = (Vector2)transform.position + direction * (_lockOnDistance * 0.5f);

        Gizmos.color = Color.cyan;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(castCenter, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.matrix = Matrix4x4.identity;
    }
}
