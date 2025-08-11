using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKey : MonoBehaviour
{
    [SerializeField] private LayerMask _objectLayer; //defino la layer del objeto que me interesa para detectar el trigger (OnPositionObject)
    [SerializeField] private PuzzleManager _puzzleManager; //Instancia del Manager asociada a un GameObject. El objeto del puzzle con el que interactue, debe tener la misma referencia a esta misma instancia para funcionar (agrupar)
    [SerializeField] private Sprite _bubbleSprite;
         //Instancia del Manager asociada a un GameObject. El objeto del puzzle con el que interactue, debe tener la misma referencia a esta misma instancia para funcionar (agrupar)
    private Collider2D _collider2D;
    public Sprite BubbleSprite { get { return _bubbleSprite; } }


    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _objectLayer) != 0)
        {
            Debug.Log("OBJETO SOBRE RESOLUCION, USADO");
            _puzzleManager.PuzzleCount();
            _collider2D.enabled = false;
            Destroy(gameObject);
        }
    }

}
