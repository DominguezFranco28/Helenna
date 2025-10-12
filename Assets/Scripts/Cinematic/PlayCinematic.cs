using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCinematic : MonoBehaviour
{
    [SerializeField] private CinematicController _cinematicController;
    private DialogueTrigger _dialogueTrigger;

    private void Start()
    {
        _dialogueTrigger = GetComponent<DialogueTrigger>();
    }
    public void Play()
    {
        _cinematicController.PlayCinematic();
        SFXManager.Instance.StopLoop();
 
    }
    
}
