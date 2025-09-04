using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubbleTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _dialogBubbleUI; 
    private bool _playerColision = false;
    public bool PlayerColision { get { return _playerColision; } }

    void Start()
    {
        _dialogBubbleUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OldPlayer")|| other.CompareTag("ChildPlayer"))
        {
            _dialogBubbleUI.SetActive(true); // Muestra la burbuja
            _playerColision = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OldPlayer") || other.CompareTag("ChildPlayer"))
        {
            _dialogBubbleUI.SetActive(false); // Oculta la burbuja
            _playerColision = false;
        }
    }

} 

