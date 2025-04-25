using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
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

            Collected.flowerValue += 1;
        }
    }

    private void OnDestroy()
    {
        hasCollected = false;
    }
}
