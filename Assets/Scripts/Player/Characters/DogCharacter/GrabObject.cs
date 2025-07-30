using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header(" Layer que debe tener el objeto para ser agarrable ")]
    [SerializeField] private LayerMask _objectLayer;
    [SerializeField] public GameObject _grabSpawnPoint;
    [SerializeField] private Sprite _bubbleSprite;
    private Sprite _originalSprite;
    private SpriteRenderer _sprite;
    private GameObject _pickedObject = null;
    private Vector2 _onPositionTransform;
    private bool _onPosition = false;


    //prop lectura y escritura porque se modifica desde el state
    public GameObject PickedObject { get { return _pickedObject; }}
    //props solo lectura porque necesita estar informado el state, pero no modificar
    public Vector2 OnPositionTransform{ get { return _onPositionTransform; } }
    public bool OnPosition { get { return _onPosition; }}
    private bool _inColision = false;
    public bool InColision { get { return _inColision; } }

    private bool _justDropped = false;// necesario xq sino me agarraba el primer item de todos cuando queria agarrar otro item, tiene que ver con el triggerstay, al soltarlo es como que volvio a colisionar con el y lo seteaba denuevo al mimsom
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_justDropped) return; //no dejamos que setee un gameobject si lo acaba de soltar, para que no se quede permanente
        //lo pase a deteccion de layer para poder hacerlo mas customizable con harold y el perro
        if (((1 << collision.gameObject.layer) & _objectLayer) !=0)
        {
                _inColision = true;
            if (_pickedObject == null /*&& Input.GetKey(KeyCode.E)*/)
            {
                _pickedObject = collision.gameObject;
                _sprite = collision.GetComponent<SpriteRenderer>();
                _originalSprite = _sprite.sprite;
            }
          
                
        }
        if (collision.gameObject.CompareTag("Ladder Position"))
        {
            _onPosition = true;
            _onPositionTransform = collision.gameObject.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _objectLayer) != 0)
        {
            _inColision = false;
            _justDropped = false;


        }
            if (collision.gameObject.CompareTag("Ladder Position"))
        {
            _onPosition = false;
        }
    }
    public void GrabItem()
    {
        if (_pickedObject == null)
        {
            return;

        }
      
            _pickedObject.transform.position = _grabSpawnPoint.transform.position;
            _pickedObject.gameObject.transform.SetParent(_grabSpawnPoint.gameObject.transform); //set the parent so follow de mouth 
            _pickedObject.GetComponent<Rigidbody2D>().simulated = false;
            _sprite.sprite = _bubbleSprite;          
        
            //como apago el rigidbody mientras lo muevo, no deberia tener problemas de fisicas. No hace falta el Fixed

    }
    public void DropItem()
    {
        if (_pickedObject != null )
        {
            ChangeSprite();
            if (!_onPosition)
                _pickedObject.tag = "Untagged";
            if (_onPosition)
            {

                _pickedObject.tag = "Climbable"; //ESTO NECESARIO para que no te puedas trepar con la nena si la escalera esta en el pios
                _pickedObject.transform.position = _onPositionTransform;
                
            }
             //para que le saque la tag a la escalera y la nena nio pueda escalar en el piso
            _pickedObject.transform.SetParent(null); //I set the parent to null, so it "drops" it
            _pickedObject.GetComponent<Rigidbody2D>().simulated = true;
            _justDropped = true; //para que el trigger no hinche
            _pickedObject = null;
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
}
