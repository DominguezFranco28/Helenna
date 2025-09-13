using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLineController : MonoBehaviour
{
    [SerializeField] private Texture[] _textures;
    [SerializeField] private GameObject _endSpritePrefab;
    [SerializeField] private float _offsetArm;
    private EdgeCollider2D _edgeCollider;
    private LineRenderer _lineRenderer;
    private GameObject _endInstance;
    private Transform _target;

    [SerializeField] private float _fps = 30f; 
    private int _animationStep;
    private float _fpsCounter;
    //control del linerenderer

    //propiedades para acceder desde el el state de nina a las posiciones de la zipline
    public Vector3 StartPoint => _lineRenderer.GetPosition(0);
    public Vector3 EndPoint => _lineRenderer.GetPosition(1);




    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();

    }
    public void AssignTarget (Vector3 startPosition, Transform newTarget)
    {
        _lineRenderer.positionCount = 2; //SEGUNDA POSICION DEL LINE RENDERER para el anclaje
        _lineRenderer.SetPosition(0, startPosition);
        _target = newTarget; //se guarda el transform del punto de anclaje

        if (_endSpritePrefab != null)
        {
            Vector3 direction = (_target.position - startPosition).normalized;
            _endInstance = Instantiate
            ( 
            _endSpritePrefab,
            _target.position,
            Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)
            ); //formula para rotar el prefab de la mano en la direccion del anclaje
            _endInstance.transform.position = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1) - direction * _offsetArm;
            //direction es el vector unitario de la linea,  si lo multiplico por offsetAmount lo aleja de la posicion final
        }
    }
   public void CancelLine() //para apagarlo desde el armimpulser y que no me quede mas de una tirolesa puesta en simultaneo
    {

        // Destruyo el sprite de la punta si existe
        if (_endInstance != null)
        {
            Destroy(_endInstance);
            _endInstance = null;
        }

        // Destruyo el LineRenderer (el mismo GameObject)
        Destroy(gameObject);

        Debug.Log("Linea destruida");
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            // si hay objetivo asignado que actualice la posicion final del linerenderer
            _lineRenderer.SetPosition(1, _target.position);

            // actualiza el sprite del final brazo()
            if (_endInstance != null)
            {
                Vector3 direction = (_target.position - _lineRenderer.GetPosition(0)).normalized;
                _endInstance.transform.position = _target.position - direction * _offsetArm;
            }

            // actualiza el collider, SOLO si hay objetivo asignado (daba bug). Revisar, hecho con IA porque desconocia los edge colliders.
            Vector3[] positions = new Vector3[2]; // array temporal para guardar las 2 posiciones del linerenderer
            _lineRenderer.GetPositions(positions);
            Vector2[] colliderPoints = new Vector2[2];
            for (int i = 0; i < positions.Length; i++)
                colliderPoints[i] = transform.InverseTransformPoint(positions[i]);
            _edgeCollider.points = colliderPoints;
        }

        // manejo de la animacion del linerenderer (probablemente se borre en version final)
        //_fpsCounter += Time.deltaTime;
        //if (_fpsCounter >= 1f / _fps)
        //{
        //    _animationStep++;
        //    if (_animationStep >= _textures.Length) _animationStep = 0;

        //    // Usar un MaterialPropertyBlock para no reiniciar el LineRenderer
        //    MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        //    _lineRenderer.GetPropertyBlock(mpb);
        //    mpb.SetTexture("_MainTex", _textures[_animationStep]);
        //    _lineRenderer.SetPropertyBlock(mpb);

        //    _fpsCounter = 0f;
        //}

        //version anterior del tuto
        //_fpsCounter += Time.deltaTime;
        //if (_fpsCounter >= 1f / _fps) 
        //{ 
        //    _animationStep++;
        //    if (_animationStep == _textures.Length)
        //        _animationStep = 0; 
        //    _lineRenderer.material.SetTexture("_MainTex", _textures[_animationStep % _textures.Length]);
        //    _fpsCounter = 0f; 
        //}
    }

}
