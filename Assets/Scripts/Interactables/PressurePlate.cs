using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private LayerMask _objectLayer;
    [SerializeField] private GameObject _interact;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _objectLayer) != 0)
        {
            Debug.Log("OBJETO SOBRE LA PLACA");
            //detect box & stop it
            IActiveable activate = _interact.GetComponent<IActiveable>();
            if (activate != null)
            {
                activate.Activate();
            }
        }
    }
}
