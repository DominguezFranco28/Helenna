using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPuzzle : Puzzle, IPuzzleObserver
{
    //Script generico para resolucion de puzzle finales que consiste en activar animacion y dezbloquear un nuevo area
    [SerializeField] MovablePlatform _movablePlatform;//esto es para el puzzle del bridge, tengo que ver de individualizarlo despues si hace falta
    private Animator _animator;
    private Collider2D _collider2D;
    //Queda pendiente la refactorizacion. Una clase abstracta base para que las hijas distingan entre objetivos del pzuzle, para no tener que poner tantos condicionales con comparacion de tags

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        if (_puzzleManager != null)
            _puzzleManager.RegisterObserver(this);
        else
            Debug.LogWarning("No se asignó un PuzzleManager a " + gameObject.name);
    }
    public void OnPuzzleEvent(int delta)
    {

        _currentCount+= delta;
        Debug.Log("resuelta una pieza del puzzle");
        Debug.Log(_currentCount);
        if (_currentCount == 0)
        {
            PuzzleSolved();
        }
    }
    public void PuzzleSolved()
    {
        Debug.Log("PUZZLE RESUELTO1");

        _animator.SetBool("Open", true);
        _collider2D.enabled = false;
        if (_movablePlatform) //esto es para el puzzle del bridge, tengo que ver de individualizarlo despues si hace falta
        {
             _collider2D.enabled = true;
            _movablePlatform.ActiveLever = true;
            _movablePlatform.ChangePosition = !_movablePlatform.ChangePosition;
        }

    }
    private void OnDestroy()
    {
        if (_puzzleManager != null)
            _puzzleManager.UnregisterObserver(this); //esto es para dejar de observar una vez resuelto el puzzle.
                                                     //Lei que puede dar problemas a futuro (temas memoria o bugs)asi que ya lo arreglo de entrada
    }
}
