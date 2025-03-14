using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InteractNPC : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private bool hasSpoken = false;
    [SerializeField] private bool inRange = false;
    [SerializeField] private GameObject noti;
    [SerializeField] private GameObject dialogue;

    [Header("Data Track")]
    [SerializeField] private int npcId;

    void Start()
    {
        
    }

    void Update()
    {
        if (InputManager.isInteractTriggered && !hasSpoken && inRange)
        {
            dialogue.SetActive(true);

            noti.SetActive(false);

            hasSpoken = true;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            if (!hasSpoken)
            {
                inRange = true;

                noti.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            noti.SetActive(false);

            inRange = false;
        }
    }
}
