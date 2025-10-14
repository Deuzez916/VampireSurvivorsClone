using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [HideInInspector]
    public PreviousScene previousScene;

    [Header("UI References")]
    public Toggle musicToggle;
    public Toggle soundToggle;

    void Start()
    {
        AudioSource bgMusic = Soundmanager.Instance.GetAudioSource(SoundEffects.BackgroundSound);
        if (bgMusic != null)
            musicToggle.isOn = bgMusic.mute == false;

        soundToggle.isOn = true;

        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        AudioSource bgMusic = Soundmanager.Instance.GetAudioSource(SoundEffects.BackgroundSound);
        if (bgMusic != null)
            bgMusic.mute = !isOn;
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        foreach (SoundEffects effect in Enum.GetValues(typeof(SoundEffects)))
        {
            if (effect == SoundEffects.BackgroundSound) continue;

            AudioSource source = Soundmanager.Instance.GetAudioSource(effect);
            if (source != null)
                source.mute = !isOn;
        }
    }

    public void OnBackPressed()
    {
        Soundmanager.Instance.PlaySoundEffect(SoundEffects.MenuBackSound);

        if (previousScene == PreviousScene.GameScene)
        {
            SceneManager.UnloadSceneAsync("settingsScene");

            StageManager.Instance.SetState(GameState.Play);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
