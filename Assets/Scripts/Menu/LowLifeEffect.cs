using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowLifeEffect : MonoBehaviour, IObserver
{
    //Interfaz de observador, porque quiero que este pendiente al StatsManager, para que haga efecto
    //de peligro cuando el jugador tenga poca vida
    public GameObject lowLife;
    public void OnNotify(int energy, int health)
    {
        var newEnergy = energy;

        if (health < 2)
        {
            lowLife.SetActive(true);
            Debug.Log("vida baja!");
        }
        else if (health > 1)
        {

            lowLife.SetActive(false);
        }

    }
     private void Start()
    {
        StatsManager.Instance.RegisterObserver(this); //Se registra como observador
    }




}
