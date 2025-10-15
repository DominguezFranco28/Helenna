using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isSpeaking = false;
    private bool pauseMenuOpen = false;
    private bool cutsceneRunning = false;


    private void CheckState()
    {
        // El orden importa, mientras mas abajo, mayor prioridad para pisar lo anterior. Son Ifs separados en  vez de else if para que se pisen entre si.

        if (cutsceneRunning || isSpeaking)
        {
            InputManager.Instance.LockInputs();
            InputManager.Instance.UnlockDialogueInputs();
        }

        if (pauseMenuOpen)
        {
            InputManager.Instance.LockInputs();
            InputManager.Instance.LockDialogueInputs();
        }
        else
        {
            InputManager.Instance.UnlockDialogueInputs();
        }


        if(!cutsceneRunning && !isSpeaking && !pauseMenuOpen)
        {
            InputManager.Instance.UnlockInputs();
            InputManager.Instance.UnlockDialogueInputs();
        }
    }

    public void SetIsSpeaking(bool value)
    {
        isSpeaking = value;
        CheckState();
    }
    public void SetPauseMenuOpen(bool value)
    {
        pauseMenuOpen = value;
        CheckState();
    }
    public void SetCutsceneRunning(bool value)
    {
        cutsceneRunning = value;
        CheckState();
    }
}
