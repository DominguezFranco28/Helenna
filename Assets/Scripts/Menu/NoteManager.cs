using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteManager : MonoBehaviour
{
    //singleton for note management
    public static NoteManager Instance { get; private set; }

    [SerializeField] private GameObject _noteUI;
    [SerializeField] private TextMeshProUGUI _noteTextUI;

    private string _nextSceneName;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Always hide the UI at the start between scenes.
        _noteUI.SetActive(false);
        DontDestroyOnLoad(Instance);
    }
    
    public void ShowNote(string text, string sceneName)
    {

        //Need to add a function that turn down the volume of adaptive music, like ActivationDoor
        GameStateManager.Instance.SetState(GameState.ReadingNote); // Turn on Reading enum from GameStateManager
        Time.timeScale = 0f;
        _noteUI.SetActive(true);
        _noteTextUI.text = text;
        _nextSceneName = sceneName;
    }

    public void CloseNote()
    {
        //idem
        GameStateManager.Instance.SetState(GameState.Playing);
        Time.timeScale = 1f;
        _noteUI.SetActive(false);
        SceneManager.LoadScene(_nextSceneName);
    }
}