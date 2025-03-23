using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCloser : MonoBehaviour
{
    [Header ("Basics")]
    [SerializeField] private GameObject dialogue;

    [Header("Drops")]
    [SerializeField] private bool isSilver = false;
    [SerializeField] private bool isFlower = false;
    [SerializeField] private bool isAbility = false;
    [SerializeField] private int dropAmount;

    [Header("Player Check")]
    public PlayerController playerController;

    [Header("Audio")]
    [SerializeField] private AudioClip closeFX;

    void Start()
    {
        Time.timeScale = 0;

        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (InputManager.isInteractTriggered)
        {
            Time.timeScale = 1;

            SoundFXManager.instance.PlaySoundClip(closeFX, transform, 1f);

            dialogue.SetActive(false);

            if (isSilver)
            {
                Collected.currencyValue = Collected.currencyValue + dropAmount;
            }

            if (isFlower)
            {
                Collected.flowerValue = Collected.flowerValue + dropAmount;
            }

            if (isAbility)
            {
                playerController._hasAbility = true;
            }
        }
    }
}
