using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Silver : MonoBehaviour
{
    private bool hasCollected = false;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player") && hitInfo.gameObject.layer != LayerMask.NameToLayer("Collider") && !hasCollected)
        {
            hasCollected = true;
            Destroy(gameObject);

            Collected.currencyValue += 1;
        }
    }

    private void OnDestroy()
    {
        hasCollected = false;
    }
}
