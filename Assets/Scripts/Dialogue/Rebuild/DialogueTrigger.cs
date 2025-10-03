using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool canBeTriggered = true;
    public string searchOnTag = "player";
    public string excludeOnTag = "none";
    public string sceneToTrigger = "";

    private void Start()
    {
        dialogueManager = GameObject.FindFirstObjectByType<DialogueManager>();

        if (GetComponent<SpriteRenderer>()) GetComponent<SpriteRenderer>().enabled = false;
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
}
