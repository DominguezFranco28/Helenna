using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class CinematicController : MonoBehaviour
{
    public TimeLineTrigger timeLineTrigger;
    public CameraZoom cameraZoom;
    public LetterboxUI letterboxUI;
    public GameObject cameraFollow;
    public bool disableCamera= true; //esto lo agrege porque apra el pet the dog uso la main camera ya que sigue a nina, no quiero desabilitarla despues del pet
    public bool playOnStart = false;
    public SpriteRenderer character; //opcional, para reestablecer flip a nina a priori.
    [Header("Fin de nivel")]
    public bool endLevel;



    [Header("Pos Forzada de personajes")]
    public List<Transform> characters; //asignar los personajes en el inspector
    public Transform cinematicPoint; //punto donde se alinean los personajes

    private PauseManager pauseManager;

    private void Start()
    {
        pauseManager = FindAnyObjectByType<PauseManager>();

        if (playOnStart)
        {
            PlayCinematic();
        }
    }
    public void AlignCharacters() //testeo alinear desde el inspector
    {
        Align(characters, cinematicPoint);
    }
    public void Align(List<Transform> characters, Transform target,float spacing = 0.3f)
    {
        if (characters.Count < 3) return;

        for (int i = 1; i < characters.Count; i++)
        {
            Vector3 targetPosition = new Vector3(
                characters[0].localPosition.x + (i * spacing), // Espaciado progresivo
                characters[i].localPosition.y,
                characters[0].localPosition.z
            );

            characters[i].localPosition = targetPosition;
        }
        if (target != null && characters.Count > 0)
        {
            characters[1].position = target.position; 
            characters[2].position = target.position; 

        }

    }

    public void PlayCinematic()
    {
        if (disableCamera)
            cameraFollow.SetActive(true);
        letterboxUI.ShowBorders();
        cameraZoom.StartZoom();

        if(pauseManager)
            pauseManager.SetCutsceneRunning(true);

        if (timeLineTrigger != null)
            timeLineTrigger.PlayTimeline();
        // El Timeline ya no se dispara desde CameraZoom cuando termina el zoom, ver comentario
    }

    public void EndCinematic()
    {
        letterboxUI.HideBorders();
        cameraZoom.ResetZoom();
        if (disableCamera)
            cameraFollow.SetActive(false);
        if (character !=null)
            character.flipX = false; //reestablezco flip a nina

        if (pauseManager)
            pauseManager.SetCutsceneRunning(false);

    }
    public void PrepareCinematic() //para los shakes de harold, sin necesidad de activar la cienmatica completa y romper todo con el timeline
    {
        if (disableCamera)
            cameraFollow.SetActive(true); // activa cámara antes del zoom
        letterboxUI.ShowBorders();
    }


}
