using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDoor : MonoBehaviour, IActiveable
{
    [SerializeField] private LayerMask objectLayer;
/*    [SerializeField] private GameObject interact; *///Que efecto realiza la placa cuando se acciona, por ejemplo, romper una puerta o disparar cinematica
    private Animator animator;
    private Collider2D collider2D;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
    }

    public void Activeable()
    {
        Debug.Log("ME ACTIVASTE!");
        animator.SetBool("Open", true);
        Destroy(collider2D);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & objectLayer) != 0)
        {

            Debug.Log("OBJETO SOBRE LA PLACA");
            Activeable();
            Destroy(other.gameObject);

            //LOGICA PARA DETENER CAJA AA MODO DE "PLACA DE PRESION"
            //Freno el movimiento del objeto que entre, y desactivo su componente que lo hace movible para el caso del viejo.
            //MovableObject movable = other.GetComponent<MovableObject>();

            //if (movable != null)
            //{
            //    movable.enabled = false;  
            //}
        }
    }
}
