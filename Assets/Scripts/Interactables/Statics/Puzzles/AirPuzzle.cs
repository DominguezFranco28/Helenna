using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPuzzle : MonoBehaviour, IPuzzleObserver
{
    [SerializeField] private int _requiredCount = 3;
    [SerializeField] private PuzzleManager _puzzleManager;
    [SerializeField] private GameObject _child; // objeto a destruir, seguramente el g.o que tiene el trigger q no deja pasar al jugador
    [SerializeField] private GameObject _interruptor; // objeto a destruir, seguramente el g.o que tiene el trigger q no deja pasar al jugador
    [SerializeField] private GameObject _door; // objeto a destruir, seguramente el g.o que tiene el trigger q no deja pasar al jugador
    private PlayCinematic _cinematic;
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
        _cinematic = GetComponent<PlayCinematic>();


    }
    public void OnPuzzleEvent(int delta)
    {
        _currentCount+=delta;

        if (_currentCount == 1)
        {
            Debug.Log("resuelta una pieza del puzzle" + _currentCount);
            {
                Debug.Log("Me hara falta mis repuestos");
            }
        }
        if (_currentCount == 2)
        {

            Destroy(_child);
        }

        if (_currentCount == 3)
        {
            _cinematic.Play();
            _interruptor.tag = "Lever"; //le pongo la tag de lever a estar altura del puzzle para que harold pueda ioneractuar con ella.
       

        }
        if (_currentCount == 4)
        {
            Debug.Log("puedes avanzar a la siguiente zona");
            PuzzleSolved();
        }
    } 

    private void OnDestroy()
    {
        if (_puzzleManager != null)
            _puzzleManager.UnregisterObserver(this); //esto es para dejar de observar una vez resuelto el puzzle.
                                                     //Lei que puede dar problemas a futuro (temas memoria o bugs)asi que ya lo arreglo de entrada
    }

    //public void AddNewPlayer() 
    //{
    //    ChildPlayerBehaviour child = FindAnyObjectByType<ChildPlayerBehaviour>();
    //    if (child != null)
    //    //    CharacterManager.Instance.JoinToTeam(child.gameObject);
    //    else
    //        Debug.LogWarning("No se encontró ningún objeto del tipo PlayerJoin en la escena.");
    //}

    void PuzzleSolved()
    {
                Debug.Log("Arreglaste el filtro de aire, ahor apodes acceder a la tercer zona");
        //    Destroy(gameObject);
        //ahora si le prendo el script a la palacan del nviel 2 para que se pueda seguir luego del puzzle nivel 1
        IActiveable action = _door.GetComponent<IActiveable>();
        action.Activate();
        //AddNewPlayer(); //esto deberia llamarlo desde un trigger
    }

    void IPuzzleObserver.PuzzleSolved()
    {
        PuzzleSolved();
    }
}
