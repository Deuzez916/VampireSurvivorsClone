using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum GameState
{
    Play,
    Pause,
    Upgrade
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

    public void SetState(GameState newState)
    {
        currentState = newState;
    }

    public bool IsPaused => currentState == GameState.Pause;
    public bool isPLaying => currentState == GameState.Play;
    public bool IsUpgrading => currentState == GameState.Upgrade;
}
