using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundEffects
{
    PlayerShoot,
    PlayerDeath,
    EnemyShoot,
    EnemyHit,
    BackgroundSound,
    MenuBackSound,
    LevelUp,
    GameOver,
    ZombieDeath,
    SnakeDeath
}

[Serializable]
public struct SoundInstance
{
    public SoundEffects Effects;
    [SerializeField] private AudioSource source;

    public AudioSource Source => source;

    public void PlaySound()
    {
        if (source != null && !source.isPlaying)
            source.Play();
    }
}

public class Soundmanager : MonoBehaviour
{
    public static Soundmanager Instance;

    [SerializeField] private List<SoundInstance> soundInstances = new();

    private SoundEffects currentBackgroundSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllBackgrounds();

        if (scene.name == "MainMenu" || scene.name == "SettingsScene")
        {
            PlayBackground(SoundEffects.MenuBackSound);
        }
        else if (scene.name == "GameScene")
        {
            PlayBackground(SoundEffects.BackgroundSound);
        }
    }

    public void PlayBackground(SoundEffects effect)
    {
        AudioSource source = GetAudioSource(effect);
        if (source != null && !source.isPlaying)
        {
            source.Play();
        }
    }

    private void StopAllBackgrounds()
    {
        foreach (var instance in soundInstances)
        {
            if (instance.Effects == SoundEffects.BackgroundSound ||
                instance.Effects == SoundEffects.MenuBackSound ||
                instance.Effects == SoundEffects.GameOver)
            {
                if (instance.Source != null)
                    instance.Source.Stop();
            }
        }
    }

    public void PlaySoundEffect(SoundEffects anEffect)
    {
        foreach (var instance in soundInstances)
        {
            if (instance.Effects == anEffect && instance.Source != null)
            {
                instance.PlaySound();
                return;
            }
        }
    }

    public AudioSource GetAudioSource(SoundEffects soundEffects)
    {
        foreach (var instance in soundInstances)
        {
            if (instance.Effects == soundEffects)
                return instance.Source;
        }

        return null;
    }

    public void StopAllSounds()
    {
        foreach (var instance in soundInstances)
        {
            if (instance.Source != null)
                instance.Source.Stop();
        }
    }

    public void StopSound(SoundEffects effect)
    {
        AudioSource source = GetAudioSource(effect);
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }
}
