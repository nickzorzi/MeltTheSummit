using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Silver : MonoBehaviour
{
    private bool hasCollected = false;
    [SerializeField] private AudioClip pickup;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player") && hitInfo.gameObject.layer != LayerMask.NameToLayer("Collider") && !hasCollected)
        {
            hasCollected = true;
            Destroy(gameObject);

            SoundFXManager.instance.PlaySoundClip(pickup, transform, 1f);

            Collected.currencyValue += 1;
        }

        if (hitInfo.CompareTag("Shock"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        hasCollected = false;
    }
}
