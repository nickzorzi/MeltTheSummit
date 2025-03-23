using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioSource musicTrack;

    [SerializeField] private AudioClip[] musicClips;

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

    private void Start()
    {
        musicTrack = GetComponent<AudioSource>();
    }

    public void SetMusicTrack(int trackNumber)
    {
        musicTrack.clip = musicClips[trackNumber];
    }
}
