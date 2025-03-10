using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    private bool hasCollected = false;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player") && hitInfo.gameObject.layer != LayerMask.NameToLayer("Collider") && !hasCollected)
        {
            hasCollected = true;
            Destroy(gameObject);

            Collected.flowerValue += 1;
        }
    }

    private void OnDestroy()
    {
        hasCollected = false;
    }
}
