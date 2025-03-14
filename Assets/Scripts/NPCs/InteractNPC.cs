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

    private void OnEnable()
    {
        if (SpawnData.Instance != null && SpawnData.Instance.npcs != null)
        {
            var npc = SpawnData.Instance.npcs.Find(e => e.id == npcId);
            if (npc != null && npc.spokenTo == true)
            {
                hasSpoken = true;
            }
        }
    }

    void Start()
    {
        if (SpawnData.Instance != null && SpawnData.Instance.npcs.Find(e => e.id == npcId) == null)
        {
            SpawnData.Instance.AddNPC(gameObject, npcId, false);
        }
    }

    void Update()
    {
        if (InputManager.isInteractTriggered && !hasSpoken && inRange)
        {
            dialogue.SetActive(true);

            noti.SetActive(false);

            hasSpoken = true;

            SpawnData.Instance.npcs.Find(e => e.id == npcId).spokenTo = true;
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
