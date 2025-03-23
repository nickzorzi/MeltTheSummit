using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    [Header("Audio")]
    [SerializeField] private AudioClip shockFX;

    void Start()
    {
        SoundFXManager.instance.PlaySoundClip(shockFX, transform, 1f);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Silver"))
        {
            Instantiate(enemy, transform.position, Quaternion.identity);

            //SoundFXManager.instance.PlaySoundClip(shockFX, transform, 1f);
        }
    }
}
