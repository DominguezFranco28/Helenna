using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool canBeTriggered = true;

    [Header("Tag Settings")]
    public string searchOnTag = "player";
    public string excludeOnTag = "none";

    [Header("Dialogue Settings")]
    public string sceneToTrigger = "";
    public int triggerLineIndex = -1;
    public int triggerShakeLineIndex = -1;

    [Header("Cinematic Trigger Settings")]
    public bool doHaroldBrake = false;
    public PlayCinematic playCinematic;
    private bool cinematicStarted = false;

    [Header("Shake Settings")]
    public CinematicController _cinematicController;
    public CameraZoom zoomShake;

    private void Start()
    {
        dialogueManager = GameObject.FindFirstObjectByType<DialogueManager>();

        if (GetComponent<SpriteRenderer>()) GetComponent<SpriteRenderer>().enabled = false;

        if (dialogueManager != null && triggerLineIndex >= 0 && playCinematic != null)
        {
            // suscripcion al evento de lineas que agregue del DialogueManager
            dialogueManager.OnLineStarted += CheckTriggerLine;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggered)
        {
            if (collision.tag.ToLower().Contains(searchOnTag) && !collision.tag.ToLower().Contains(excludeOnTag))
            {
                canBeTriggered = false;
                dialogueManager.StartScene(sceneToTrigger);
            }
        }
    }
    private void CheckTriggerLine(DialogueLine line)
    {
        // solo disparar si el trigger está configurado
        if (playCinematic == null || triggerLineIndex < 0)
            return;
     //   Debug.Log($"Revisando línea {line.lineId} de {line.scene} (esperada: {triggerShakeLineIndex})");

        if (line.scene == sceneToTrigger && line.lineId == triggerShakeLineIndex)
        {
            cinematicStarted = true;
            _cinematicController.PrepareCinematic(); // camara + letterbox
            zoomShake.TriggerHaroldBreak();
        }
        if (line.scene == sceneToTrigger && line.lineId == triggerLineIndex)
        {
            if (!cinematicStarted)//no quiero que dispare dos veces la cinematica si la llama desde el shake
            {
                cinematicStarted = true;
                playCinematic.Play();
              //  Debug.Log($"Cinemática disparada en línea {line.lineId} de {line.scene}");
            }
            dialogueManager.OnLineStarted -= CheckTriggerLine;
        }
    }
}
