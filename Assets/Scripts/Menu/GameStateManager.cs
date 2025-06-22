using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState { Playing, Paused, ReadingNote }
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
    }

    public bool IsGamePaused()
    {
        return CurrentState != GameState.Playing;
    }
}
