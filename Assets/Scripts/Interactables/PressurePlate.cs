using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private LayerMask objectLayer;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & objectLayer) != 0)
        {

            Debug.Log("OBJETO SOBRE LA PLACA");

            //LOGICA PARA DETENER CAJA AA MODO DE "PLACA DE PRESION"
            //Freno el movimiento del objeto que entre, y desactivo su componente que lo hace movible para el caso del viejo.
            MovableObject movable = other.GetComponent<MovableObject>();

            if (movable != null)
            {
                movable.enabled = false;
            }
        }
    }
}
