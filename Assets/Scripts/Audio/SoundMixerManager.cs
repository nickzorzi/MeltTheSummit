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
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public void SetMasterVolume(float level)
    {
        //audioMixer.SetFloat("masterVolume", level);

        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);
    }

    public void SetSoundFXVolume(float level)
    {
        //audioMixer.SetFloat("soundFXVolume", level);

        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        //audioMixer.SetFloat("musicVolume", level);

        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
    }
}
