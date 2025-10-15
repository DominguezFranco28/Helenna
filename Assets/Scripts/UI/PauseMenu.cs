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

    public bool isPaused = false;
    private bool canTogglePause = true;

    private PauseManager pauseManager;

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

        ContinueGame();
        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    private void TogglePauseGame()
    {
        if (canTogglePause)
        {
            canTogglePause = false;
            if (isPaused)
            {
                StartCoroutine(ContinueGameRoutine());
            }
            else
            {
                StartCoroutine(PauseGameRoutine());
            }
        }
    }

    private IEnumerator PauseGameRoutine()
    {
        ShowMenu();
        isPaused = true;
        if (pauseManager)
            pauseManager.SetPauseMenuOpen(isPaused);

        yield return new WaitForSeconds(0.1f);
        canTogglePause = true;
    }

    private void ContinueGame()
    {
        StartCoroutine(ContinueGameRoutine());
    }

    private IEnumerator ContinueGameRoutine()
    {
        HideMenu();
        isPaused = false;
        if (pauseManager)
            pauseManager.SetPauseMenuOpen(isPaused);

        yield return new WaitForSeconds(0.1f);
        canTogglePause = true;
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
