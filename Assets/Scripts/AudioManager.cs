using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        // Убедитесь, что только один AudioManager существует в игре
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Сделаем объект постоянным между сценами
    }

    private void Start()
    {
        LoadVolumeSettings(); // Загружаем настройки при старте игры
    }

    public void SetMusicVolume(float volume)
    {
        // Убедитесь, что параметр "MusicVolume" существует в AudioMixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVolume", volume); // Сохраняем значение
        }
    }

    public void SetSFXVolume(float volume)
    {
        // Убедитесь, что параметр "SFXVolume" существует в AudioMixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume); // Сохраняем значение
        }
    }

    private void LoadVolumeSettings()
    {
        // Загружаем сохраненные значения или используем значения по умолчанию
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }
}