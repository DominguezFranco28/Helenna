using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteManager : MonoBehaviour
{
    //Singleton para gestion de notas
    public static NoteManager Instance { get; private set; }

    [SerializeField] private GameObject _noteUI;
    [SerializeField] private TextMeshProUGUI _noteTextUI;

    private string _nextSceneName;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //Que comience siempre oculta la UI entre escenas.
        _noteUI.SetActive(false);
        DontDestroyOnLoad(Instance);
    }
    
    public void ShowNote(string text, string sceneName)
    {
        GameStateManager.Instance.SetState(GameState.ReadingNote); //Activamos estado de lectura del GameStateManager
        Time.timeScale = 0f;
        _noteUI.SetActive(true);
        _noteTextUI.text = text;
        _nextSceneName = sceneName;
    }

    public void CloseNote()
    {
        GameStateManager.Instance.SetState(GameState.Playing);
        Time.timeScale = 1f;
        _noteUI.SetActive(false);
        SceneManager.LoadScene(_nextSceneName);
    }
}