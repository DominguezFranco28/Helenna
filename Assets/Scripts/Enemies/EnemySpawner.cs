using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : PlayerDetector
{

    //Creacion del Pool en el spawner que va a gestionar las bolas de fuego.
    [SerializeField] private FireBall fireBallPrefab;
    ObjectPool<FireBall> poolFireBalls;
    // Esto del ObjectPool es gracias al seteado de la API que tiene unity para el patrn Pool. Setea el object pool mas rapido, deja de ser necesario hacerlo desde 0}
    // Revisar recurso lectura pag 52.
    [SerializeField] private Transform spawnPoint;//Lo defino en la clase, porque sino despues el Invoke Repeating me chilla por
                                                  //no pasarle parametro en el metodo Spawnear

    private void Awake()
    {
        poolFireBalls = new ObjectPool<FireBall>(CreatePoolItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, 1, 10);
    }
    private FireBall CreatePoolItem()
    {
        FireBall newFireBall = Instantiate(fireBallPrefab);
        newFireBall.gameObject.SetActive(false);
        newFireBall.pool = poolFireBalls;

        return newFireBall;
    }

    private void OnTakeFromPool(FireBall fireBall)
    {
        fireBall.gameObject.SetActive(true); //El fireball.enabled daba problemas. Fue mejor modificar directo el gameobject, no solo el componente.
    }
    private void OnReturnedToPool(FireBall fireBall)
    {
        fireBall.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(FireBall fireBall)
    {
        Destroy(fireBall);
    }
    private void SpawnFireballs()
    { // Verificamos si podemos crear más bolas de fuego. Un chek de seguridad, nunca realmente necesario en mi caso.
        if (poolFireBalls.CountAll >= 10 && poolFireBalls.CountInactive == 0)
        {
            Debug.Log("Pool lleno - esperando a que se liberen bolas"); //Seguramente termine borrando
            return;
        }
        FireBall fireBall = poolFireBalls.Get();
        if (fireBall == null) { return; }      
        fireBall.transform.position = spawnPoint.transform.position;
        fireBall.InitializeDirection(); //Una vez spaawnea la fireball, ahi ejecuta su direccion asi no da problemas de posicionamiento.
    }
    public override void Effect(Collider2D collision) 
    {
        InvokeRepeating("SpawnFireballs", 1, 1f);
        //Una funcion que INVOCA repetidamente, lo hace cada 1segs. Por eso no hay necesidad de ponerlo en el update
        //Me la traje a esta firma obligatoria heredada de la clase abstracta, porque quiero que el spawner comience a funcionar cuando el jugador llega a cierta zona,
        //no desde elñ inicio. 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CancelInvoke("SpawnFireballs"); //SI EL PLAYER sale de la zona del trigger, se detiene el spawn, es decir, me frezea el pool.
        }
    }
}
    
