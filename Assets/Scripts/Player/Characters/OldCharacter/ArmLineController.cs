using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLineController : MonoBehaviour
{
    [SerializeField] private Texture[] _textures;
    [SerializeField] private GameObject _endSpritePrefab;
    private LineRenderer _lineRenderer;
    private GameObject _endInstance;
    private Transform _target;

    [SerializeField] private float _fps = 30f; 
    private int _animationStep;
    private float _fpsCounter;
    //control del linerenderer
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    public void AssignTarget (Vector3 startPosition, Transform newTarget)
    {
        _lineRenderer.positionCount = 2; //SEGUNDA POSICION DEL LINE RENDERER para el anclaje
        _lineRenderer.SetPosition(0, startPosition);
        _target = newTarget; //se guarda el transform del punto de anclaje

        if (_endSpritePrefab != null)
        {
            Vector3 direction = (_target.position - startPosition).normalized;
            GameObject endInstance = Instantiate
            ( 
            _endSpritePrefab,
            _target.position,
            Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)
            ); //formula para rotar el prefab de la mano en la direccion del anclaje
        }
    }
   public void CancelLine() //para apagarlo desde el armimpulser y que no me quede mas de una tirolesa puesta en simultaneo
    {
        _lineRenderer.positionCount = 0; // Oculta la línea
        _target = null;

        if (_endInstance != null)
        {
            Destroy(_endInstance);
            _endInstance = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.SetPosition(1, _target.position); //actualiza la posicion del linerenderer en cada frame

        if (_endInstance != null) //actualiza la posicion del prefab de la mano en el punto de anclaje
            _endInstance.transform.position = _target.position;

        _fpsCounter += Time.deltaTime;
        if (_fpsCounter >=1f / _fps)
        {
            _animationStep++;
            if ( _animationStep == _textures.Length)
                _animationStep = 0; 
            _lineRenderer.material.SetTexture("_MainTex", _textures[_animationStep % _textures.Length]);
            _fpsCounter = 0f;
        }
    }
}
