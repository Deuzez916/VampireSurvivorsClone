using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;
    public Toggle soundToggle;
    public Toggle musicToggle;

    [Header("Controls")]
    public Slider controlSchemeSlider;

    void Start()
    {
        soundToggle.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        musicToggle.isOn = !musicSource.mute;
        controlSchemeSlider.value = PlayerPrefs.GetInt("ControlScheme", 0);
    }

    public void ToggleSound(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
        PlayerPrefs.SetInt("SoundOn", isOn ? 1 : 0);
    }

    public void ToggleMusic(bool isOn)
    {
        musicSource.mute = !isOn;
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
    }

    public void SetControlScheme(float value)
    {
        int scheme = Mathf.RoundToInt(value);
        PlayerPrefs.SetInt("ControlScheme", scheme);
        Debug.Log("Control Scheme: " + (scheme == 0 ? "Left-Handed" : "Right-Handed"));
    }
}
