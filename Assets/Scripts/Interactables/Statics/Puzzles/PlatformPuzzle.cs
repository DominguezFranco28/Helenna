using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPuzzle : Puzzle, IPuzzleObserver
{
    [SerializeField] MovablePlatform _movablePlatform;
    protected override void Start() //overrida necesario para sobreeescribir el start
    {
        base.Start(); //seteo el required count heredado de la clase abstracta, y luego agrego logica ind
        if (_puzzleManager != null)
            _puzzleManager.RegisterObserver(this);
        else
            Debug.LogWarning("No se asignó un PuzzleManager a " + gameObject.name);
    }
    public void OnPuzzleEvent()
    {
        Debug.Log("PLATAFORMA ACTIVADA");
        SFXManager.Instance.PlaySFX(_SFX);
        //cada vez que termina el movimiento reestablese a false dentro del script.
        if (_movablePlatform != null)
        {
            _movablePlatform.ActiveLever = true; //no solo era importante swicehar la direcion tambioen tenia que indicarle si se habia ejecutado la palanca
            _movablePlatform.ChangePosition = !_movablePlatform.ChangePosition; // esto invierte la boleana! de true a false y vicebersa
        }
    }

    public void PuzzleSolved()
    {
        throw new System.NotImplementedException();
    }
}
