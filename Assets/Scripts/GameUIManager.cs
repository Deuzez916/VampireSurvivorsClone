using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public Button pauseButton;
    public Sprite pauseIcon;
    public Sprite PlayIcon;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;

    public GameObject settingsPanel;

    [Header("Pause Menu Buttons")]
    public Button settingsButton;
    public Button quitButton;

    private Image pauseIconImage;

    void Start()
    {
        pauseIconImage = pauseButton.transform.Find("Icon").GetComponent<Image>();

        pauseMenu.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (StageManager.Instance != null)
            StageManager.Instance.Reset();

        pauseButton.interactable = true;
        UpdateButtonIcon();

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitToMainMenu);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
    }


void Update()
    {
        if (StageManager.Instance.IsGameOver)
        {
            if (gameOverPanel != null && !gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);
                pauseButton.interactable = false;
            }
        }
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

    public void QuitToMainMenu()
    {
        if (StageManager.Instance != null)
            StageManager.Instance.Reset();

        if (Soundmanager.Instance != null)
            Soundmanager.Instance.StopAllSounds();

        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        StageManager.Instance.Reset();

        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        Soundmanager.Instance.StopSound(SoundEffects.BackgroundSound);

        Soundmanager.Instance.PlaySoundEffect(SoundEffects.MenuBackSound);

        StageManager.Instance.SetState(GameState.Pause);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        Soundmanager.Instance.StopSound(SoundEffects.MenuBackSound);

        Soundmanager.Instance.PlaySoundEffect(SoundEffects.BackgroundSound);
        
        settingsPanel.SetActive(false);
        StageManager.Instance.SetState(GameState.Pause);
    }
}
