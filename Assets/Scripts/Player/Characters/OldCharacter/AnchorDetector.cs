using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorDetector : MonoBehaviour
{
    [SerializeField] private float _lockOnRadius = 10f;
    [SerializeField] private LayerMask _anchorLayer;
    [SerializeField] private GameObject _UI;
    [SerializeField] private CinemachineVirtualCamera _cameraFollow;
    private ArmImpulser _oldPlayerBehaviour;
    private Transform _closestAnchor = null;

    public Transform ClosestAnchor { get { return _closestAnchor; } }
    public AnchorType CurrentAnchor { get; private set; }

    //propr publicty para retornar el tipo de anclaje del punto de anclaje seleccionado

    private void Start()
    {
        _oldPlayerBehaviour = GetComponent<ArmImpulser>();
    }
    private void Update()
    {
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _lockOnRadius, _anchorLayer); //detetcta todos los colliders dentro de un circulo centrado en la posicion del jugador y con un radio definido
        Transform closestAnchor = null;
        float minDist = Mathf.Infinity; //inicializa la variable de distancia minima con infinito, para garantizar que cualquier distancia medida en el foreach sea menor

        foreach (var hit in hits) //cada hit que encuentre de la layer asignada, va a medirlo con el anterior en termino de distnacia, el mas cercano se guarda en closestAnchor
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestAnchor = hit.transform;
            }
        }
        return closestAnchor; //retorno el transfor del objeto mas cercano
    }
    public void LockOnToAnchor(Transform anchor) //enfoca al punto de anclaje retornado
    {
        if (anchor == null) return;   

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _lockOnRadius);
    }
}
