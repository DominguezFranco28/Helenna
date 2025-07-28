using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPuzzle : MonoBehaviour, IPuzzleObserver
{
    [SerializeField] private int _requiredCount = 3;
    [SerializeField] private PuzzleManager _puzzleManager;
    [SerializeField] private IActiveable _activateDoor; 
    [SerializeField] private GameObject _child; // arrastrás el hijo desde el inspector

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
        if (gameObject.CompareTag("Movable Platform"))
        {
            Debug.Log("PLATAFORMA ACTIVADA");
            MovablePlatform platform = gameObject.GetComponent<MovablePlatform>();
            //cada vez que termina el movimiento reestablese a false dentro del script.
            if (platform != null)
            {
                platform.ActiveLever = true; //no solo era importante swicehar la direcion tambioen tenia que indicarle si se habia ejecutado la palanca
                platform.ChangePosition = !platform.ChangePosition; // esto invierte la boleana! de true a false y vicebersa
                return;
            }
        }

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
        Debug.Log("PUZZLE RESUELTO1"); 
        //retocar tdoo esto para no tener parametros vacios con todos los puzzles
        if (gameObject.CompareTag("Activatable Anchor"))
        {
            Debug.Log("activaste el anclaje");
            _child.SetActive(true);
            //OnDestroy();
        }
        else if (_activateDoor != null)
        {
            _activateDoor.Activate();
            OnDestroy();
        }

        else
         {
            //este es el del filtro del aire. Creo siwtch, hago condiconales, o separo scripts?
            Destroy(_child);
        }
      
    }
    private void OnDestroy()
    {
        if (_puzzleManager != null)
            _puzzleManager.UnregisterObserver(this); //esto es para dejar de observar una vez resuelto el puzzle.
                                                     //Lei que puede dar problemas a futuro (temas memoria o bugs)asi que ya lo arreglo de entrada
    }

}
