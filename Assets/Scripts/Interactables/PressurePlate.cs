using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IActiveable
{
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private GameObject interact; //Que efecto realiza la placa cuando se acciona, por ejemplo, romper una puerta o disparar cinematica
    private BoxCollider2D _collider2D;

    public void Activeable()
    {
        Debug.Log("ME ACTIVASTE!");
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 360);
        Destroy(interact);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & objectLayer) != 0)
        {
            Debug.Log("OBJETO SOBRE LA PLACA");
            //Freno el movimiento del objeto que entre
            MovableObject movable = other.GetComponent<MovableObject>();
            if (movable != null)
            {
                movable.enabled = false;  // Desactivo el componente para frenar el movimiento
                Activeable();
            }
        }
    }
}
