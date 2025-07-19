using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private List<IPuzzleObserver> observers = new List<IPuzzleObserver>();// Lista de observadores pendientes de cambios en el Script. Es a quienes se les notifica de algun cambio 
    private void Start()
    {

    }
    //Metódo para REGISTRAR un observador que lo solicite.
    public void RegisterObserver(IPuzzleObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }
    // Método para eliminar un observador
    public void UnregisterObserver(IPuzzleObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }
    // Métodos para actualizar info y notificar a los observadores
    public void PuzzleCount()
    {
        NotifyObservers();
    }


    // Notificar a todos los observadores
    private void NotifyObservers()
    {
        foreach (IPuzzleObserver observer in observers)
        {
            observer.OnPuzzleEvent();
        }
    }
}
