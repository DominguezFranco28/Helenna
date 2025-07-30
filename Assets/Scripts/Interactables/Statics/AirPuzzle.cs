using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPuzzle : MonoBehaviour, IPuzzleObserver, IActiveable
{
    [SerializeField] private int _requiredCount = 3;
    [SerializeField] private PuzzleManager _puzzleManager;
    [SerializeField] private GameObject _child; // objeto a destruir, seguramente el g.o que tiene el trigger q no deja pasar al jugador
    [SerializeField] private GameObject _nextChild; // objeto a destruir, seguramente el g.o que tiene el trigger q no deja pasar al jugador

    private int _currentCount = 0;


    public int CurrentCount { get { return _currentCount; } }


    void Start()
    {
         //detecta si el objeto que tiene este script tiene la interfaz de activable para llamar a su metodo con el solution puzzle (en las puertas x ej para activar la animacion)
        ; //Esta iniciacion en start me va a permitir instanciar diferentes clases y que no se activen con el mismo
        if (_puzzleManager != null)
            _puzzleManager.RegisterObserver(this);
        else
            Debug.LogWarning("No se asignó un PuzzleManager a " + gameObject.name);

        
    }
    public void OnPuzzleEvent()
    {
        _currentCount+=1;

        Debug.Log("resuelta una pieza del puzzle" + _currentCount) ;
        {
            Debug.Log("Me hara falta mis repuestos");
            
        }
        if (_currentCount == 2)
        {

            Destroy(_child);
        }

        if (_currentCount == _requiredCount)
        {
            Debug.Log("puedes avanzar a la siguiente zona");
            PuzzleSolved();
        }
    } 

    private void PuzzleSolved()
    {
        Debug.Log("Arreglaste el filtro de aire, ahor apodes acceder a la tercer zona");
        //ahora si le prendo el script a la palacan del nviel 2 para que se pueda seguir luego del puzzle nivel 1
        _nextChild.GetComponent<Collider2D>().enabled = true; 


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
