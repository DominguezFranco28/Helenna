using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TransitionManager manager;
    public string firstLevel = "";
    public Button playButton;

    private void Start()
    {
        if (playButton)
            playButton.onClick.AddListener(StartGame);
        if (manager)
            manager.nextScene = firstLevel;
    }

    private void StartGame()
    {
        if (manager)
            manager.ChangeLevel();
    }
}
