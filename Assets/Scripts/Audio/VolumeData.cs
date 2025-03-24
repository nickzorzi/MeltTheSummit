using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeData : MonoBehaviour
{
    public static VolumeData instance;

    public float masterLevel;
    public float sfxLevel;
    public float musicLevel;

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
}
