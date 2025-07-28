using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPuzzle : MonoBehaviour, IPuzzleObserver, IActiveable
{
    [SerializeField] private int _requiredCount = 3;
    [SerializeField] private PuzzleManager _puzzleManager;
    [SerializeField] private GameObject _child; // objeto a destruir, seguramente el g.o que tiene el trigger q no deja pasar al jugador

    private int _currentCount;


    void Start()
    {
         //detecta si el objeto que tiene este script tiene la interfaz de activable para llamar a su metodo con el solution puzzle (en las puertas x ej para activar la animacion)
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
        {
            Debug.Log("Me hara falta mis repuestos");
            
        }
        if (_currentCount == 1)
        {

            Destroy(_child);
        }

        if (_currentCount == 0)
        {
            Debug.Log("puedes avanzar a la siguiente zona");
            PuzzleSolved();
        }
    } 

    private void PuzzleSolved()
    {

        Debug.Log("Arreglaste el filtro de aire");
        //aca supongo que iria el llamado a activate

    }
    private void OnDestroy()
    {
        if (_puzzleManager != null)
            _puzzleManager.UnregisterObserver(this); //esto es para dejar de observar una vez resuelto el puzzle.
                                                     //Lei que puede dar problemas a futuro (temas memoria o bugs)asi que ya lo arreglo de entrada
    }

    public void Activate()
    {
        Debug.Log("activada la maquina del aire en su totalidad!");
       //anim del filtro de aire andando. Y casa?
    }
}
