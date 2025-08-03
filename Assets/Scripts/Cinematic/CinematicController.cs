using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    public CameraZoom cameraZoom;
    public LetterboxUI letterboxUI;

    public void PlayCinematic()
    {
        letterboxUI.ShowBorders();
        cameraZoom.StartZoom();
        // El Timeline se dispara desde CameraZoom cuando termina el zoom.
    }

    public void EndCinematic()
    {
        letterboxUI.HideBorders();
        cameraZoom.ResetZoom();
    }
}
