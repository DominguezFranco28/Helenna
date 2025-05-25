using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RevealSecret : MonoBehaviour
{
    [SerializeField] private GameObject secretToReveal; //Elijo desde el inspector el objeto que esta ocultando la zona secreta.
    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position; //Establezo la pos inicial para luego hacer la comparacion de distancia.
       
    }

    

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, initialPos); //Metodo distance util, sirve para mediir distancia entre punto a y b. 
        //Al estar en un update, en todo momento esta chequeando la distancia que hay entre la ubicacion del objeto, y su posición inicial.
            
            if (distance > 0.5f && secretToReveal != null) //Si la distancia es mayor a la que establezca aca, me revela el objeto.
            {

            Destroy(secretToReveal);
            secretToReveal = null; //Esto es para evitar que se hagan mas de un llamado al Destroy, despues de destruido el secreto (ya que el objeto con el script sigue en juego)

            }
        
        }
}
