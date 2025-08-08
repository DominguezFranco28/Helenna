using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private LayerMask _objectLayer; //defino la layer del objeto que me interesa para detectar el trigger (OnPositionObject)
    [SerializeField] private PuzzleManager _puzzleManager; //Instancia del Manager asociada a un GameObject. El objeto del puzzle con el que interactue, debe tener la misma referencia a esta misma instancia para funcionar (agrupar)
    [SerializeField] private AudioClip _audioClip;
    private bool _platformUsed = false;
    private Collider2D _collider2D;
    private Animator _animator;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _objectLayer) != 0 && !_platformUsed)
        {
            Debug.Log("Placa de presion activada");
            _platformUsed = true;

            SFXManager.Instance.PlaySFX(_audioClip);
            _animator.SetBool("IsPressed", true);
            _puzzleManager.PuzzleCount();
            // detect box &stop it / activate lever
            MovableObject movable = other.GetComponent<MovableObject>();

            if (movable != null)
            {
                movable.StopMove(transform.position);
            }
            _collider2D.enabled = false;
            //aca agregaria anims o cambios de sprite, si tan solo los tuviera ;c
        }
    }

}