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

    private LanguageManager languageManager;
    public Button esButton;
    public Button enButton;
    
    public Button touchModeButton;

    private void Start()
    {
        if (playButton)
            playButton.onClick.AddListener(StartGame);
        if (quitButton)
            quitButton.onClick.AddListener(ExitGame);
        if (esButton)
            esButton.onClick.AddListener(ChangeLanguage);
        if (enButton)
            enButton.onClick.AddListener(ChangeLanguage);
        if (manager)
            manager.nextScene = firstLevel;
        if (touchModeButton)
            touchModeButton.onClick.AddListener(ToggleTouchMode);

        languageManager = LanguageManager.Instance;
        if (languageManager)
        {
            SetLanguageButtons();
        }
    }

    public void ChangeLanguage()
    {
        switch (languageManager.currentLanguage)
        {
            case 0://ES
                languageManager.currentLanguage = 1;
                break;
            case 1://EN
                languageManager.currentLanguage = 0;
                break;
        }
        SetLanguageButtons();
    }

    public void SetLanguageButtons()
    {
        if(esButton && enButton)
        {
            switch (languageManager.currentLanguage)
            {
                case 0://ES
                    esButton.interactable = false;
                    enButton.interactable = true;
                    break;
                case 1://EN
                    esButton.interactable = true;
                    enButton.interactable = false;
                    break;
            }
            
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("main-menu", "playButton");
            quitButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("main-menu", "quitButton");
            if (InputManager.Instance)
            {
                if (OnScreenControlsManager.Instance.onScreenButtonsEnabled)
                {
                    touchModeButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("main-menu", "touchMode-on");
                }
                else
                {
                    touchModeButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("main-menu", "touchMode-off");
                }
            }
            else
            {
                Debug.LogError("Input system not found");
            }
        }
        
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

    public void ToggleTouchMode()
    {
        if (OnScreenControlsManager.Instance)
        {
            OnScreenControlsManager.Instance.onScreenButtonsEnabled = !OnScreenControlsManager.Instance.onScreenButtonsEnabled;
            if (OnScreenControlsManager.Instance.onScreenButtonsEnabled)
            {
                touchModeButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("main-menu", "touchMode-on");
            }
            else
            {
                touchModeButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("main-menu", "touchMode-off");
            }
        }
        else
        {
            Debug.LogError("Input system not found");
        }
    }
}
