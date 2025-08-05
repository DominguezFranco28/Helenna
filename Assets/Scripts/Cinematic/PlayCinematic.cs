using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCinematic : MonoBehaviour
{
    [SerializeField] private CinematicController _cinematicController;
    [SerializeField] private OldPlayerBehaviour _oldPlayerBehaviour;
    public void Play()
    {
        _oldPlayerBehaviour.StopMovement();
        _cinematicController.PlayCinematic();
        SFXManager.Instance.StopLoop();
 
    }
}
