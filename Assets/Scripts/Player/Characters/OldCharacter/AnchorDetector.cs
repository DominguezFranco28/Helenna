using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorDetector : MonoBehaviour
{
    [SerializeField] private float _lockOnRadius = 10f;
    [SerializeField] private LayerMask _anchorLayer;
    private ArmImpulser _oldPlayerBehaviour;
    private Transform _closestAnchor = null;

    public Transform ClosestAnchor { get { return _closestAnchor; } }
    //propr de lectura para tener info para los states

    private void Start()
    {
        _oldPlayerBehaviour = GetComponent<ArmImpulser>();
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
        //        Debug.Log("Locking on to anchor: " + anchor.name);
        // aca iria logica de UI pop ups etc
        //aca tambien podria ir la logica que me swithcee entre distintos puntos de anclaje (dezplazamiento, tirolesas)
        //hacer un enum para tipos de anclaje 
        //logica de deslizamiento dentro del impulsSTATE
    }
    void Update()
    {
        _closestAnchor = DetectClosestAnchor(); //el return del metodo asigna el punto de anclaje mas cercano a la variable anchor
        if (_closestAnchor != null)
        {
            LockOnToAnchor(_closestAnchor);        
        }       
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _lockOnRadius);
    }
}
