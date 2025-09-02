using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    [SerializeField] private CinematicController _cinematicController;

    private void Start()
    {
        // esto es para suscribirse al evento cuando termina la timeline, sino no detecta el stopped
        _director.stopped += OnTimelineStopped;
    }

    public void PlayTimeline()
    {
        // if (!_hasActivate)
        _director.Play();
    }
    // Este metodo se llamna automatico cuando termine la timeline asignada
    private void OnTimelineStopped(PlayableDirector pd)
    {
        RestoreValues();
    }
    private void RestoreValues()
    {
        // puedo ponerle para restauraro el control
        GameStateManager.Instance.SetState(GameState.Playing);
        Debug.Log("Timeline terminó. Restaurando valores originales...");
        _cinematicController.EndCinematic();
    }

    private void OnDestroy()
    {
        // Limpia la suscripción para evitar errores si el objeto se destruye
        _director.stopped -= OnTimelineStopped;
    }
}
