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
    public Button quitButton;

    private void Start()
    {
        if (playButton)
            playButton.onClick.AddListener(StartGame);
        if (quitButton)
            quitButton.onClick.AddListener(ExitGame);
        if (manager)
            manager.nextScene = firstLevel;
    }

    private void StartGame()
    {
        if (manager)
            manager.ChangeLevel();
    }
    private void ExitGame()
    {
        Application.Quit();
    }
}
