using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject entranceBox;
    [SerializeField] private GameObject backtrackBox;

    public PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player._inBossRoom = true;

            Destroy(entranceBox);

            backtrackBox.SetActive(false);

            dialogue.SetActive(true);
        }
    }
}
