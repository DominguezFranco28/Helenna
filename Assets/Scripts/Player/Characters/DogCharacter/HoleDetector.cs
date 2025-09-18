using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class HoleDetector : MonoBehaviour
{
   [SerializeField] private AgilePlayerBehaviour _playerBehaviour;
   [SerializeField] private AgilePlayerController _playerController;
    [SerializeField] private string _waterLayerName = "Water";
    private AgileStateMachine _stateMachine;
    private int _waterLayer;
    public bool IsInWater { get; private set; } //esto para usar el el throw state y detecte si esta en el agua o no

    private void Awake()
    {
        if (_playerController != null)
        {
            _stateMachine = _playerController.StateMachine; //capto la misma referencia de la maquina de estados, no la inicio de nuevo
        }
        _waterLayer = LayerMask.NameToLayer(_waterLayerName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //I turn off the fence collider when the dog detects the hole
        if (collision.CompareTag("Hole"))
        {
            Transform parent = collision.transform.parent;
            if (parent != null)
            {
                Collider2D parentCollider = parent.GetComponent<Collider2D>();
                if (parentCollider != null)
                {
                    parentCollider.enabled = false;
                }
            }
            _playerBehaviour.CanDig = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Hole"))
        {
            Transform parent = collision.transform.parent;
            if (parent != null)
            {
                Collider2D parentCollider = parent.GetComponent<Collider2D>();
                if (parentCollider != null)
                {
                    parentCollider.enabled = true;
                }
            }
            _playerBehaviour.CanDig = false;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _waterLayer && _stateMachine.CurrentState == _stateMachine.thrownState)
        {
            Debug.Log("Rex entered water while being thrown, ignoring collision.");
            Collider2D rexCollider = GetComponent<Collider2D>();
            Collider2D waterCollider = collision.collider;

            Physics2D.IgnoreCollision(rexCollider, waterCollider, true);
            IsInWater = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _waterLayer)
        {
            Debug.Log("Rex entered water while being thrown, ignoring collision.");
            Collider2D rexCollider = GetComponent<Collider2D>();
            Collider2D waterCollider = collision.collider;

            Physics2D.IgnoreCollision(rexCollider, waterCollider, false);
            IsInWater = false;
        }
    }
}




