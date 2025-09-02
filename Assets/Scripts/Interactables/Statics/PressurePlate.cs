using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private LayerMask _objectLayer; //defino la layer del objeto que me interesa para detectar el trigger (OnPositionObject)
    [SerializeField] private PuzzleManager _puzzleManager; //Instancia del Manager asociada a un GameObject. El objeto del puzzle con el que interactue, debe tener la misma referencia a esta misma instancia para funcionar (agrupar)
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _needHold;

    private Collider2D _collider2D;
    private Animator _animator;
    public bool NeedHold { get { return _needHold; } } //prop solo lectura, la uso para detectar en el puzzle que use placa de presion en conjunto.
                                                       //Cuando se resuelve el puzzle, las apaga a todas las relacionadas llamando al metodo publico

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _objectLayer) != 0 )
        {
            Debug.Log("Placa de presion activada");

            SFXManager.Instance.PlaySFX(_audioClip);
            _animator.SetBool("IsPressed", true);
            
            if(_puzzleManager)
                _puzzleManager.PuzzleCount(-1); //resto uno al contador si se presiona

            // detect box &stop it / activate lever
            MovableObject movable = other.GetComponent<MovableObject>();

            if (movable != null)
            {
                movable.StopMove(transform.position);
            }
            if (!_needHold)
            {

              _collider2D.enabled = false;
                //como una no necesita ser mantenida, la "apago" por mas que ya no tenga un objeto encima.
            }
          
            //aca agregaria anims o cambios de sprite, si tan solo los tuviera ;c
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       // if (!_needHold) return; // si no necesita mantenerse, ignoro el exit para que no me rompa la animacion
        if (((1 << other.gameObject.layer) & _objectLayer) != 0 && _needHold)
        {
            Debug.Log("Placa de presion desactivada");

            _animator.SetBool("IsPressed", false);
          //  _collider2D.enabled = true;
            _puzzleManager.PuzzleCount(+1); // De ser de las placas de presion que requieren estar apretadas en simultaneo,
                                           // SUMO uno al count si se sale de la placa, para obligar al jugador a pulsarla en simultaneo con otra para alcanzar la cuenta requerida dle puzzle.
            
            //aca agregaria anims o cambios de sprite, si tan solo los tuviera ;c
        }
    }
    public void DeactivatePlate()
    {
        //Metodo publico para que el puzzle pueda apagar la placa de presion, por ejemplo si se resuelve el puzzle y se quiere apagar todas las placas de presion
        _needHold = false; //desactivo la necesidad de mantener presionada la placa
    }


}