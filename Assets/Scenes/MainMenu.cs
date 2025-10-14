using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        Soundmanager.Instance.PlaySoundEffect(SoundEffects.BackgroundSound);
    }

    public void OpenSettings()
    {
        Soundmanager.Instance.StopAllSounds();

        Soundmanager.Instance.PlaySoundEffect(SoundEffects.MenuBackSound);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        Soundmanager.Instance.StopAllSounds();

        Soundmanager.Instance.PlaySoundEffect(SoundEffects.MenuBackSound);
        settingsPanel.SetActive(false);
    }
}