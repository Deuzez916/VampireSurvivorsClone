using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button pauseButton;
    public Sprite pauseIcon;
    public Sprite PlayIcon;

    private Image pauseIconImage;

    void Start()
    {
        pauseIconImage = pauseButton.transform.Find("Icon").GetComponent<Image>();
        pauseButton.onClick.AddListener(TogglePause);
        pauseMenu.SetActive(false);

        UpdateButtonIcon();
    }

    void TogglePause()
    {
        if (StageManager.Instance.currentState == GameState.Play)
        {
            PauseGame();
        }
        else if (StageManager.Instance.currentState == GameState.Pause)
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        StageManager.Instance.SetState(GameState.Pause);
        pauseMenu.SetActive(true);
        UpdateButtonIcon();
    }

    void ResumeGame()
    {
        StageManager.Instance.SetState(GameState.Play);
        pauseMenu.SetActive(false);
        UpdateButtonIcon();
    }

    void UpdateButtonIcon()
    {
        if (pauseIconImage != null)
            pauseIconImage.sprite = StageManager.Instance.IsPaused ? PlayIcon : pauseIcon;
    }
}
