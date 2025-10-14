using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum GameState
{
    Play,
    Pause,
    Upgrade,
    GameOver,
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public GameState currentState = GameState.Play;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    public void Reset()
    {
        currentState = GameState.Play;
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
    }

    public bool IsPaused => currentState == GameState.Pause;
    public bool IsPLaying => currentState == GameState.Play;
    public bool IsUpgrading => currentState == GameState.Upgrade;
    public bool IsGameOver => currentState == GameState.GameOver;
}
