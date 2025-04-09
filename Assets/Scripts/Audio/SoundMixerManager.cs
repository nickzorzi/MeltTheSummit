using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager instance;

    [SerializeField] private AudioMixer audioMixer;


    private void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        if (VolumeData.instance.masterLevel == 0)
        {
            SetMasterVolume(1);
            SetSoundFXVolume(1);
            SetMusicVolume(0.2f);
        }
        else
        {
            SetMasterVolume(VolumeData.instance.masterLevel);
            SetSoundFXVolume(VolumeData.instance.sfxLevel);
            SetMusicVolume(VolumeData.instance.musicLevel);
        }
    }

    public void SetMasterVolume(float level)
    {
        //audioMixer.SetFloat("masterVolume", level);

        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);

        VolumeData.instance.masterLevel = level;
    }

    public void SetSoundFXVolume(float level)
    {
        //audioMixer.SetFloat("soundFXVolume", level);

        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);

        VolumeData.instance.sfxLevel = level;
    }

    public void SetMusicVolume(float level)
    {
        //audioMixer.SetFloat("musicVolume", level);

        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);

        VolumeData.instance.musicLevel = level;
    }
}
