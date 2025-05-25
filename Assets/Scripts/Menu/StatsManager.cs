using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
public class StatsManager : MonoBehaviour
{
    private List<IObserver> observers = new List<IObserver>();// Lista de observadores pendientes de cambios en el StatsManager. Es a quienes se les notifica de algun cambio en los Stats
    private PlayerEnergy playerEnergy;  //Declaro las variables correspondientes de esus clases, para modificar el valor directo en sus clases y no tenes incossitencias en la UI y los valores reales del PJ.
    private PlayerHealth playerHealth;
    private TelekinesisForce playerPower;
    public static StatsManager instance;
    public static StatsManager Instance => instance;

    private void Awake()
    {
        if (instance ==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //Puedo hacer que bserve la victoria y derrota tambien

    private void Start()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealth = GetComponent<PlayerHealth>();
        playerPower = GetComponent<TelekinesisForce>();
    }
    //Metódo para REGISTRAR un observador que lo solicite.
    public void RegisterObserver(IObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }
    // Método para eliminar un observador
    public void UnregisterObserver(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }
    // Métodos para actualizar la energia y notificar a los observadores
    public void NewEnergy(int newEnergy)
    {
        playerEnergy.Energy += newEnergy;
        NotifyObservers();       
    }
  

    // Métodos para actualizar la vida y notificar a los observadores 
    public void NewHealth(int newHealth)
    {
        playerHealth.Health += newHealth;
        NotifyObservers();
    }

    //Metodo para actualizar el powerUp
    public void UsePowerUp (int power)
    {
        playerPower.PushForce += power;
        NotifyObservers();
    }
    // Notificar a todos los observadores
    private void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(playerEnergy.Energy, playerHealth.Health, playerPower.PushForce);
        }
    }
}
