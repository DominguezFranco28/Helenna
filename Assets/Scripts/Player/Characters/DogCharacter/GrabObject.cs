using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header(" Layer que debe tener el objeto para ser agarrable ")]
    [SerializeField] private LayerMask _objectLayer;
    [SerializeField] public GameObject _grabSpawnPoint;
    [SerializeField] private Sprite _originalSprite;
    [SerializeField] private Sprite _bubbleSprite;
    private SpriteRenderer _sprite;
    private GameObject _pickedObject = null;
    private Vector2 _onPositionTransform;
    private bool _onPosition = false;

    //prop lectura y escritura porque se modifica desde el state
    public GameObject PickedObject { get { return _pickedObject; }set { _pickedObject = value; } }
    //props solo lectura porque necesita estar informado el state, pero no modificar
    public Vector2 OnPositionTransform{ get { return _onPositionTransform; } }
    public bool OnPosition { get { return _onPosition; }}
    void Update()
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //lo pase a deteccion de layer para poder hacerlo mas customizable con harold y el perro
        if (((1 << collision.gameObject.layer) & _objectLayer) != 0 /*&& !_platformUsed*//* && Input.GetKey(KeyCode.E)*/)
        {
            if (_pickedObject == null /*&& Input.GetKey(KeyCode.E)*/)
            {
                _pickedObject = collision.gameObject;
                _sprite = collision.GetComponent<SpriteRenderer>();
            }
                
        }
    }
    public void GrabItem()
    {
        if (_pickedObject != null)//optra validacion redundante creo
        {
            _sprite.sprite = _bubbleSprite;
            _pickedObject.transform.position = _grabSpawnPoint.transform.position;
            _pickedObject.gameObject.transform.SetParent(_grabSpawnPoint.gameObject.transform); //set the parent so follow de mouth 
            _pickedObject.GetComponent<Rigidbody2D>().simulated = false;
            //como apago el rigidbody mientras lo muevo, no deberia tener problemas de fisicas. No hace falta el Fixed
        }
    }
    public void ChangeSprite()
    {
        _sprite.sprite = _originalSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder Position"))
        {
            _onPosition = true;
            _onPositionTransform = collision.gameObject.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder Position"))
        {
            _onPosition = false;
            
        }
    }
}
