using UnityEngine;
using UnityEngine.UI;

public class PauseButtonManager : MonoBehaviour
{
    public Button pauseButton;
    public Button PlayButton;

    void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        PlayButton.onClick.AddListener(ResumeGame);

        updateButtons();
    }

    void PauseGame()
    {
        StageManager.Instance.SetState(GameState.Pause);
        updateButtons();
    }

    void ResumeGame()
    {
        StageManager.Instance.SetState(GameState.Play);
        updateButtons();
    }

    void updateButtons()
    {
        bool isPaused = StageManager.Instance.currentState == GameState.Pause;

        pauseButton.gameObject.SetActive(!isPaused);
        PlayButton.gameObject.SetActive(isPaused);
    }
}
