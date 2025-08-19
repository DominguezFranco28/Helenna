using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPuzzle : Puzzle , IPuzzleObserver
{
    [SerializeField] GameObject _anchorChild;
    Animator _animator;
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

    public void PuzzleSolved()
    {
        Debug.Log("activaste el anclaje");
        SFXManager.Instance.PlaySFX(_SFX); //SFX HEREDADO DE PUZZLE, cada hija lo pone en el inspector

        _animator.SetBool("IsActive", true);
        
        _anchorChild.SetActive(true);
    }

    // Start is called before the first frame update
    protected override void Start() //overrida necesario para sobreeescribir el start
    {
        base.Start(); //seteo el required count heredado de la clase abstracta, y luego agrego logica ind
        _animator = GetComponent<Animator>();
        if (_puzzleManager != null)
            _puzzleManager.RegisterObserver(this);
        else
            Debug.LogWarning("No se asignó un PuzzleManager a " + gameObject.name);
    }

}
