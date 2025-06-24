using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDetector : MonoBehaviour  //CLASE ABSTRACTA. A modo de MOLDE para otros recolectables
    //Clase abstracta e interfaz son dos formas de darle abstraccion a algo
{

     public virtual void OnTriggerEnter2D(Collider2D collision) //Establezco un ""llamado comun" para sus hijas, el entrar en contacto con el Player
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {

            Effect(collision);
            
        }
    }
    public abstract void Effect(Collider2D collision); //Firma del metodo, obligatoria para sus hijas.

}
