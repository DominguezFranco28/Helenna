using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private List<IPuzzleObserver> observers = new List<IPuzzleObserver>();

    // para REGISTRAR un observador que lo solicite.
    public void RegisterObserver(IPuzzleObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }
    //  para eliminar un observador
    public void UnregisterObserver(IPuzzleObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }
    //  para actualizar info y notificar a los observadores
    public void PuzzleCount( int delta )
    {
        NotifyObservers(delta);
    }

    // Notificar a todos los observadores
    private void NotifyObservers(int delta)
    {
        foreach (IPuzzleObserver observer in observers)
        {
            observer.OnPuzzleEvent(delta);
        }
    }
}
