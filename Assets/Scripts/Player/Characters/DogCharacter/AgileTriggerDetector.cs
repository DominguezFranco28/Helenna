using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class AgileTriggerDetector : MonoBehaviour
{
    [SerializeField] private AgilePlayerBehaviour _playerBehaviour;
    [SerializeField] private AgilePlayerController _playerController;
    [SerializeField] private string _waterLayerName = "Water";
    private AgileStateMachine _stateMachine;
    private int _waterLayer;
    public bool IsInWater { get; private set; } //esto para usar el el throw state y detecte si esta en el agua o no
    public bool IsBeeingPulled{ get; set; }

    private void Awake()
    {
        if (_playerController != null)
        {
            _stateMachine = _playerController.StateMachine; //capto la misma referencia de la maquina de estados, no la inicio de nuevo
        }
        _waterLayer = LayerMask.NameToLayer(_waterLayerName);
    }
    public void IgnoreWater(bool ignore)
    {
        IsInWater = ignore;
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
        if (collision.gameObject.layer == _waterLayer)
        {
            IsInWater = true;
            Debug.Log("Rex entered water ");

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
        if (collision.gameObject.layer == _waterLayer)
        {
            IsInWater = false;
            Debug.Log("Rex exit water.");

        }
    }

}





