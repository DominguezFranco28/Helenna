using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FranciscoPonce : MonoBehaviour  //El nombre viene de un amigo que me pidio este enemigo.
{
    [SerializeField] int speed = 1;
    private Vector3 moveDirection;
    private Vector3 playerPos;
    private float distance;
    private GameObject player;

    public void InitializeDirection() //Tomo del script que gestiona el Pool
    {
        player = GameObject.FindWithTag("Player");
        playerPos = player.transform.position;
       
       
        if (player != null)
        {
            // Calcula la direccion para mvoerse hacia el jugador
            moveDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            Debug.Log("No se encontró al jugador");
            moveDirection = Vector3.down; //Una pos predefinida.
        }
    }
    void Update()
    {

        InitializeDirection();
        transform.Translate(moveDirection * Time.deltaTime * speed);
        float distance = Vector3.Distance(transform.position, playerPos);
        //Dependiendo que tan cerca este del jugador, se movera mas o menos lento. Si el jugador lo empuja lejos, este se destruye
        if (distance > 30f)
        {
            Destroy(gameObject);
        }
        else if (distance > 7f)
        {
            speed = 5;
        }
        else
        {
            speed = 2;
        }
    }

}

