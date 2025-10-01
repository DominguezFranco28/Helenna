using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    public CameraZoom cameraZoom;
    public LetterboxUI letterboxUI;
    public GameObject cameraFollow;
    public bool playOnStart = false;
    public PuzzleManagerLevel01 manager = null;

    private void Start()
    {
        if (playOnStart)
        {
            PlayCinematic();
        }
    }
    public void PlayCinematic()
    {
        cameraFollow.SetActive(true);
        letterboxUI.ShowBorders();
        cameraZoom.StartZoom();
        GameStateManager.Instance.SetState(GameState.Paused);
        // El Timeline se dispara desde CameraZoom cuando termina el zoom.
    }

    public void EndCinematic()
    {
        letterboxUI.HideBorders();
        cameraZoom.ResetZoom();
        cameraFollow.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Playing);
        if ( manager != null)
        {
            manager.Victory();
        }
    }
}
