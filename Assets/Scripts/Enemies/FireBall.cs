using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireBall : MonoBehaviour
{
    //Referencia al Pool
    public ObjectPool<FireBall> pool;


    //Referencias a la logica del jugador y direccion de la fireball
    private GameObject player;
    private StatsManager statsManager;
    private Vector3 bulletDirection;
    public void InitializeDirection() //Mejor asi antes que Star porque no era correcta la pos.
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            statsManager = player.GetComponent<StatsManager>();
            // Calcula la direccion despues de poner la fireball con el spawner.
            bulletDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            Debug.Log("No se encontró al jugador");
            bulletDirection = Vector3.down; //Una pos predefinida.
        }
    }
    void Update()
    {
        transform.Translate(bulletDirection * Time.deltaTime * 8);
    }
    private void OnEnable()
    {
            Invoke("Desactivate", 4);
    }

    public void Desactivate() //Metodo publico para llamarlo desde el EnemySpawner
    {
        // Cancelo  cualquier invocación pendiente (daba problemas con el spawner)
        CancelInvoke("Desactivate");

        //Desactivo el gameobject primero, no solo el componente enabled, marcó esta cuestión en la clase grabada 8
        gameObject.SetActive(false);

        // Lo libero al pool
        if (pool != null)
        {
            pool.Release(this);
        }
        else //Por si algo falla.
        {
            Destroy(gameObject);
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colisioné con el jugador");
            Desactivate();
            
        }
    }


}

