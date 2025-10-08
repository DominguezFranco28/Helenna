using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PuzzleManagerLevel03 : MonoBehaviour
{
    public TMP_Text endScreen;
    public DialogueManager dialogueManager;
    [Header("Cinematica")]
    public PlayCinematic playCinematic;

    [Header("Puzzle 1")]
    public DialogueTrigger dialogueTrigger;

    private void OnEnable()
    {
        dialogueTrigger.OnTriggered += DialogueTriggered;
        dialogueManager.OnFinished += TriggerVictory;
    }
    private void OnDisable()
    {
        dialogueTrigger.OnTriggered -= DialogueTriggered;
        dialogueManager.OnFinished -= TriggerVictory;
    }

    private void DialogueTriggered()
    {
        if (playCinematic)
            playCinematic.Play();
    }

    private void TriggerVictory(string scene)
    {
        if(scene == dialogueTrigger.sceneToTrigger)
            StartCoroutine(Victory());
    }

    private IEnumerator Victory()
    {
        Debug.Log("VICTORY");
        yield return new WaitForSeconds(0.1f);

        if (endScreen)
        {
            endScreen.text = "Fin de Demo";
            endScreen.gameObject.SetActive(true);

            yield return new WaitForSeconds(5f);
            TransitionManager.Instance.ChangeLevel();
        }
    }
}
