using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private GameObject interact;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & objectLayer) != 0)
        {

            Debug.Log("OBJETO SOBRE LA PLACA");

            //LOGICA PARA DETENER CAJA AA MODO DE "PLACA DE PRESION"
            IActiveable activate = interact.GetComponent<IActiveable>();

            if (activate != null)
            {
                activate.Activate();
            }
        }
    }
}
