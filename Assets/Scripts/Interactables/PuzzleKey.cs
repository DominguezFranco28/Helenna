using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKey : MonoBehaviour
{
    [SerializeField] private LayerMask _objectLayer; //defino la layer del objeto que me interesa para detectar el trigger (OnPositionObject)
    [SerializeField] private PuzzleManager _puzzleManager; //Instancia del Manager asociada a un GameObject. El objeto del puzzle con el que interactue, debe tener la misma referencia a esta misma instancia para funcionar (agrupar)


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _objectLayer) != 0)
        {
            Debug.Log("OBJETO SOBRE RESOLUCION");
            // detect box &stop it /recoredar lo de activable
            IMovable movable = other.GetComponent<IMovable>();
            IActiveable activeable = other.GetComponent<IActiveable>();
            if (movable != null)
            {
                movable.StopMove();
                _puzzleManager.PuzzleCount();
            }
            else if (activeable != null)
            {
                _puzzleManager.PuzzleCount();
            }
        }
    }

}
