using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPuzzle : Puzzle , IPuzzleObserver
{
    [SerializeField] GameObject _anchorChild;
    [SerializeField]  PressurePlate[] _pressurePlates;
    Animator _animator;
    public void OnPuzzleEvent(int delta)
    {
        _currentCount +=delta;
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
        foreach (PressurePlate pressurePlate in _pressurePlates)
        {
            if (pressurePlate.NeedHold)
            {
                pressurePlate.DeactivatePlate(); ; //desactivo las placas de presion que necesitan mantenerse activas
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start() //overrida necesario para sobreeescribir el start
    {
        base.Start(); //seteo el required count heredado de la clase abstracta, y luego agrego logica ind
        Debug.Log("Iniciando AnchorPuzzle" + _currentCount);
        _animator = GetComponent<Animator>();
        if (_puzzleManager != null)
            _puzzleManager.RegisterObserver(this);
        else
            Debug.LogWarning("No se asignó un PuzzleManager a " + gameObject.name);
    }

}
