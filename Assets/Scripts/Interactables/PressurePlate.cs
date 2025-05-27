using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private LayerMask objectLayer;
    private BoxCollider2D _collider2D;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & objectLayer) != 0)
        {
            Debug.Log("OBJETO SOBRE LA PLACA");
            //Freno el movimiento del objeto que entre
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
