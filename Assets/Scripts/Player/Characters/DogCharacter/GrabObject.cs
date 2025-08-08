using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header(" Layer que debe tener el objeto para ser agarrable ")]
    [SerializeField] private LayerMask _objectLayer;
    [SerializeField] public GameObject _grabSpawnPoint;
    [SerializeField] private Sprite _bubbleSprite;
    [SerializeField] private AudioClip _pickSFX;
    private Sprite _originalSprite;
    private SpriteRenderer _sprite;
    private GameObject _pickedObject = null;
    private Vector2 _onPositionTransform;
    private bool _onPosition = false;    
    // Lista de objetos en colision, para evitar el tp de objetos mal referenciados desde lejos.
    // Lo meto en triggfer enter, y lo saco en el exit para aseugrarme de no pcikear ese objeto por ams que este lejos
    private List<GameObject> _collidingObjects = new List<GameObject>();


    //prop lectura y escritura porque se modifica desde el state
    public GameObject PickedObject { get { return _pickedObject; }}
    //props solo lectura porque necesita estar informado el state, pero no modificar
    public Vector2 OnPositionTransform{ get { return _onPositionTransform; } }
    public bool OnPosition { get { return _onPosition; }}
    private bool _inColision = false;
    public bool InColision { get { return _inColision; } }

    private bool _justDropped = false;// necesario xq sino me agarraba el primer item de todos cuando queria agarrar otro item, tiene que ver con el triggerstay, al soltarlo es como que volvio a colisionar con el y lo seteaba denuevo al mimsom
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _objectLayer) != 0)
        {
            if (!_collidingObjects.Contains(collision.gameObject)) //agrego el item a la lista cuando colisiona con el
                _collidingObjects.Add(collision.gameObject);
            _inColision = true;
        }
        if (collision.gameObject.CompareTag("Ladder Position"))
        {
            _collidingObjects.Remove(collision.gameObject);
            _onPosition = true;
            _onPositionTransform = collision.gameObject.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _objectLayer) != 0)
        {
            if (_collidingObjects.Contains(collision.gameObject)) //lo saco de la lista cuando deja la colision, evito problemas de agarrar un item cuando estoy sonbre otro por problemas de ref
                _collidingObjects.Remove(collision.gameObject);
            _inColision = false;


        }
            if (collision.gameObject.CompareTag("Ladder Position"))
        {
            _onPosition = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder Position"))
        {
            _onPosition = true;
            _onPositionTransform = collision.gameObject.transform.position;
        }
    }
    public void GrabItem()
    {
        if (_collidingObjects.Count == 0) return;
        _pickedObject = _collidingObjects[0]; // agarro el primer objeto de la lista siempre
        _sprite = _pickedObject.GetComponent<SpriteRenderer>();
        _originalSprite = _sprite.sprite;
        PlaySFX();
        _pickedObject.transform.position = _grabSpawnPoint.transform.position;
        _pickedObject.gameObject.transform.SetParent(_grabSpawnPoint.gameObject.transform); //set the parent so follow de mouth 
        _pickedObject.GetComponent<Rigidbody2D>().simulated = false;

        if (_bubbleSprite != null)
        {

        _sprite.sprite = _bubbleSprite;          
        }
        
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
    public void PlaySFX()
     {
        SFXManager.Instance.PlaySFX(_pickSFX);
     }
}
