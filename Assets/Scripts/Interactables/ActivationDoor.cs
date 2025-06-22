using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDoor : MonoBehaviour, IActiveable
{
    [SerializeField] private LayerMask _objectLayer;
    [SerializeField] private AudioClip _openSFX;
/*    [SerializeField] private GameObject interact; *///Que efecto realiza la placa cuando se acciona, por ejemplo, romper una puerta o disparar cinematica
    private Animator animator;
    private Collider2D collider2D;
    private AdaptiveMusicLayering musicLayering;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        musicLayering = FindAnyObjectByType<AdaptiveMusicLayering>();
        
    }

    public void Activeable()
    {
        Debug.Log("ME ACTIVASTE!");
        SFXManager.Instance.PlaySFX(_openSFX);
        animator.SetBool("Open", true);
        Destroy(collider2D);
        //Manejo de las capaz de musica a modo de resolucion de puzzle
        musicLayering.FadeBaseMusicVolume(0.3f); // Baja a 30%
        musicLayering.PlayResolutionTone();
        StartCoroutine(RestoreMusicAfterDelay(musicLayering, 2f)); //llamo a corrutina para restaurar los niveles de musica
    }
    private IEnumerator RestoreMusicAfterDelay(AdaptiveMusicLayering layering, float delay)
    {
        yield return new WaitForSeconds(delay);
        layering.FadeBaseMusicVolume(0.8f); // Valor original o el que usabas antes
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _objectLayer) != 0)
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
