using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public bool forceTriggerTimer = false;
    public int startMinutes = 1; // starting time in minutes
    public int startSeconds = 30; // extra seconds if needed
    public TextMeshProUGUI timerDisplay;
    private float time;
    private bool isPaused = false;
    private Coroutine timerRoutine;

    public event System.Action OnTimerStarted;
    public event System.Action OnTimerFinished;

    public void StartTimer()
    {
        if(time <= 0.001f)
            time = startMinutes * 60 + startSeconds;
        timerRoutine = StartCoroutine(Countdown());

        OnTimerStarted?.Invoke();
    }

    public void PauseTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            UpdateTimerDisplay();
        }
    }

    public void StopTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            time = 0f;
            UpdateTimerDisplay();
        }
            
    }

    private IEnumerator Countdown()
    {
        while (time > 0f)
        {
            yield return new WaitForSeconds(1f);
            time--;
            UpdateTimerDisplay();
        }
        Debug.Log("Timer Finished");
        OnTimerFinished?.Invoke();
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerDisplay)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void Update()
    {
        if (forceTriggerTimer)
        {
            forceTriggerTimer = false;
            StartTimer();
        }

    }

    public float GetInitialTimeSeconds()
    {
        return startMinutes * 60f + startSeconds;
    }
}
