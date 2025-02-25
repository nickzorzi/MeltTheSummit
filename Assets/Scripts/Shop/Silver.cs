using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Silver : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
