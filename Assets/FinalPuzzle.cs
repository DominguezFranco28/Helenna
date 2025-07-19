using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPuzzle : MonoBehaviour, IPuzzleObserver
{
    [SerializeField] private int _requiredCount = 3;
    [SerializeField] private PuzzleManager _puzzleManager;
    [SerializeField] private IActiveable _activateDoor;
   
    private int _currentCount;
    //al final no lo hice instanciando el singleton porque no me dejaba reutilizarlo para otros puzzles.

    void Start()
    {
        _activateDoor = GetComponent<IActiveable>(); //detecta si el objeto que tiene este script tiene la interfaz de activable para llamar a su metodo con el solution puzzle (en las puertas x ej para activar la animacion)
        _currentCount = _requiredCount; //Esta iniciacion en start me va a permitir instanciar diferentes clases y que no se activen con el mismo
        if (_puzzleManager != null)
            _puzzleManager.RegisterObserver(this);
        else
            Debug.LogWarning("No se asignó un PuzzleManager a " + gameObject.name);
    }
    public void OnPuzzleEvent()
    {
        _currentCount--;
        Debug.Log("resuelta una pieza del puzzle"); 
        Debug.Log(_currentCount);
        if (_currentCount == 0)
        {
            PuzzleSolved();
        }
    }

    private void PuzzleSolved()
    {
        _activateDoor.Activate();
       // Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (_puzzleManager != null)
            _puzzleManager.UnregisterObserver(this); //esto es para dejar de observar una vez resuelto el puzzle.
                                                     //Lei que puede dar problemas a futuro (temas memoria o bugs)asi que ya lo arreglo de entrada
    }

}
