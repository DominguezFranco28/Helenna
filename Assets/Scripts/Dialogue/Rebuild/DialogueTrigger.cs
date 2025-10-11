using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool canBeTriggered = true;
    public string searchOnTag = "player";
    public string excludeOnTag = "none";
    public string sceneToTrigger = "";
    [Header("Cinematic Trigger Settings")]
    public int triggerLineIndex;

    public event System.Action onLineReached;
    public PlayCinematic playCinematic;

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

        if (line.scene == sceneToTrigger && line.lineId == triggerLineIndex)
        {
            playCinematic.Play();
            Debug.Log($"Cinemática disparada en línea {line.lineId} de {line.scene}");

            // Opcional: desuscribirse si solo querés disparar una vez
            dialogueManager.OnLineStarted -= CheckTriggerLine;
        }
    }
}
