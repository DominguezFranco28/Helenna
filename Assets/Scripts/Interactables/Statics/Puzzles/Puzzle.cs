using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    //Clase abstracta base que define parametros comunes y obligatorios para todos los scripts que gestionen un puzzle.
    //Parametros protected para que cada hija tenga su copia del campo, no compartido entre instancias. Cada puzzle maneja su contador
    [SerializeField] protected int _requiredCount;
    [SerializeField] protected PuzzleManager _puzzleManager;
    [SerializeField] protected AudioClip _SFX;

    protected int _currentCount; //variable protegida. Cada clase que herede tendra su propiio count

    protected virtual void Start()
    {
        _currentCount = _requiredCount; //valor incial, comun para todas las hijas con un base.start, luego agrego logica individual x script

    }
}
