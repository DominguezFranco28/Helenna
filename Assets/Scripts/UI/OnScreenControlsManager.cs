using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenControlsManager : MonoBehaviour
{
    public static OnScreenControlsManager Instance => _instance;
    private static OnScreenControlsManager _instance;

    public bool onScreenButtonsEnabled = false;

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
}
