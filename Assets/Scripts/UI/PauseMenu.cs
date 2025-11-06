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

    private Slider sfxSlider;
    private Slider musicSlider;
    public TextMeshProUGUI settingsTitle;

    private LanguageManager languageManager;

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
        languageManager = LanguageManager.Instance;

        if (continueButton)
            continueButton.onClick.AddListener(ContinueGame);
        if (toMenuButton)
            toMenuButton.onClick.AddListener(ToMenu);
        if (quitButton)
            quitButton.onClick.AddListener(ExitGame);

        background = GetComponent<Image>();

        ContinueGame();
        pauseManager = FindAnyObjectByType<PauseManager>();

        Slider[] sliders = GetComponentsInChildren<Slider>(true);
        if(sliders.Length > 0)
        {
            foreach (Slider slider in sliders)
            {
                if (slider.name.ToLower().Trim().Contains("music"))
                    musicSlider = slider;
                else
                    sfxSlider = slider;
            }
        }

        InitAudioSliders();

        if (languageManager)
        {
            continueButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("pause-menu", "continueButton");
            toMenuButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("pause-menu", "toMenuButton");
            quitButton.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("pause-menu", "quitButton");

            settingsTitle.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("settings-menu", "title");
            sfxSlider.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("settings-menu", "sfxLabel");
            musicSlider.GetComponentInChildren<TextMeshProUGUI>().text = languageManager.GetUIText("settings-menu", "musicLabel");
        }
    }

    private void InitAudioSliders()
    {
        AudioManager audio = AudioManager.Instance;
        if (audio)
        {
            if (sfxSlider)
            {
                sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);

                float val = audio.GetMixerGroupVolume("SFX");
                Debug.Log("Mixer: SFX="+val);
                sfxSlider.value = val;
            }
            else
                Debug.Log("Mixer: SFX slider not found");

            if (musicSlider)
            {
                musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);

                float val = audio.GetMixerGroupVolume("Music");
                Debug.Log("Mixer: Music=" + val);
                musicSlider.value = val;
            }
            else
                Debug.Log("Mixer: Music slider not found");
        }
    }

    private void OnSFXSliderValueChanged(float value)
    {
        AudioManager audio = AudioManager.Instance;
        if (audio)
            audio.SetMixerGroupVolume("SFX", value);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        AudioManager audio = AudioManager.Instance;
        if (audio)
            audio.SetMixerGroupVolume("Music", value);
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

    public void PauseGame()
    {
        StartCoroutine(PauseGameRoutine());
    }

    public void ContinueGame()
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
