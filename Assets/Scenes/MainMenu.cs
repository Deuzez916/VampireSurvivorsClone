using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SettingsMenu()
    {
        Debug.Log("Settings-knappen trycktes!");
        SceneManager.LoadScene("SettingsMenu");
    }
}
