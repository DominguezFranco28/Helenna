using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public TransitionManager manager;
    public string menuLevel = "";
    public Button continueButton;
    public Button toMenuButton;
    public Button quitButton;

    public GameObject pauseMenu;
    private Image background;

    private bool isPaused = false;
    private bool canTogglePause = true;

    private void OnEnable()
    {
        InputManager.Instance.PausePressed += TogglePauseGame;
    }

    private void OnDisable()
    {
        InputManager.Instance.PausePressed -= TogglePauseGame;
    }

    private void Start()
    {
        if (continueButton)
            continueButton.onClick.AddListener(ContinueGame);
        if (toMenuButton)
            toMenuButton.onClick.AddListener(ToMenu);
        if (quitButton)
            quitButton.onClick.AddListener(ExitGame);

        background = GetComponent<Image>();

        HideMenu();
    }

    private void TogglePauseGame()
    {
        if (canTogglePause)
        {
            canTogglePause = false;
            if (isPaused)
            {
                ContinueGame();
            }
            else
            {
                ShowMenu();
                InputManager.Instance.LockInputs();
            }
            isPaused = !isPaused;
            Invoke("AllowPauseToggle", 0.5f);
        }
        
    }

    private void AllowPauseToggle()
    {
        canTogglePause = true;
    }

    private void ContinueGame()
    {
        HideMenu();
        InputManager.Instance.UnlockInputs();
    }

    private void ToMenu()
    {
        if (manager)
        {
            ContinueGame();
            manager.nextScene = menuLevel;
            manager.ChangeLevel();
        }
            
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ShowMenu()
    {
        if (pauseMenu) pauseMenu.SetActive(true);
        if (background) background.enabled = true;
    }
    private void HideMenu()
    {
        if (pauseMenu) pauseMenu.SetActive(false);
        if (background) background.enabled = false;
    }
}
