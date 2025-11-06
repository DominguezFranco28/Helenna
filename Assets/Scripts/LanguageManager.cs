using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;


[Serializable]
public class UITextEntry
{
    public string objectLocation;
    public string objectName;
    public string text;
}

[Serializable]
public class UITextList
{
    public UITextEntry[] UItexts;
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance => _instance;
    private static LanguageManager _instance;
    private UITextList uiTextList;
    public int currentLanguage = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void LoadJSON()
    {
        string filePath;
        if (currentLanguage == 0)
            filePath = "JSON/ES/UI";
        else
            filePath = "JSON/EN/UI";

        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found");
            return;
        }

        uiTextList = JsonUtility.FromJson<UITextList>(jsonFile.text);
    }

    public string GetUIText(string objectLocation, string objectName)
    {
        LoadJSON();

        if (uiTextList == null || uiTextList.UItexts == null)
        {
            Debug.LogError("UI text list not loaded or empty.");
            return null;
        }

        var entry = uiTextList.UItexts.FirstOrDefault(x =>
            x.objectLocation == objectLocation && x.objectName == objectName);

        if (entry != null)
            return entry.text;

        Debug.LogWarning($"Text not found for {objectLocation}/{objectName}");
        return null;
    }

    public string GetCurrentLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string level = sceneName.ToLower().Trim().Split("level")[1];

        if (level.Length > 0)
        {
            Debug.Log("current level: " + level);
            return level;
        }
        else
            return "00";
    }
}
