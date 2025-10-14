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
    [SerializeField] private float _anchorScreenY = 0.35f;
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
        if (state) // Solo si estamos activando el seguimiento del anclaje
        {
            // en body de VC tiene que estar el framingntransposer seteado
            var framingTransposer = _cameraFollow.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (framingTransposer != null)
            {
                // La propiedad m_ScreenY define la pos Y del target en la pantalla.
                // Si el valor es MENOR que 0.5, el target se muestra en la parte inferior de la pantalla, creando efecto de la camara x debajo del anclaje
                framingTransposer.m_ScreenY = _anchorScreenY;

                // framingTransposer.m_DeadZoneHeight = 0.1f; 
                // para que el anclaje no se mueva
            }
        }
    
        
    }

    private void GetBoxCastParams(out Vector2 origin, out Vector2 size, out float angle, out Vector2 direction)
    {
        // Get current input direction
        Vector2 dir = _oldPlayerBehaviour.LastMovementInput;
        if (dir.sqrMagnitude < 0.001f)
            dir = _defaultAnchorDirection;
        else
            _defaultAnchorDirection = dir;
        origin = transform.position;
        size = new Vector2(_lockOnDistance, _boxWidth);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        direction = dir;
    }

    public Transform DetectClosestAnchor()
    {
        if (_oldPlayerBehaviour == null)
            return null;

        GetBoxCastParams(out Vector2 origin, out Vector2 size, out float angle, out Vector2 direction);

        // Perform BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            size,
            angle,
            direction,
            _lockOnDistance,
            _anchorLayer
        );

        return hit.collider ? hit.transform : null;
    }

    private void OnDrawGizmosSelected()
    {
        if (_oldPlayerBehaviour == null)
            return;

        GetBoxCastParams(out Vector2 origin, out Vector2 size, out float angle, out Vector2 direction);
        Vector3 castCenter = origin + (direction * _lockOnDistance * 0.5f);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(castCenter, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + direction * _lockOnDistance);
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
}
