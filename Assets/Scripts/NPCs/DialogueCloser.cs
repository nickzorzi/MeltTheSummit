using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCloser : MonoBehaviour
{
    [Header ("Basics")]
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject abilityTut;

    [Header("Drops")]
    [SerializeField] private bool isSilver = false;
    [SerializeField] private bool isFlower = false;
    [SerializeField] private bool isAbility = false;
    [SerializeField] private int dropAmount;

    [Header("Boss")]
    [SerializeField] private bool isBossEntrance = false;
    public Unit unit;
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private bool isBossPreFight = false;

    [Header("Player Check")]
    public PlayerController playerController;

    [Header("Audio")]
    [SerializeField] private AudioClip closeFX;

    void Start()
    {
        Time.timeScale = 0;

        playerController = FindObjectOfType<PlayerController>();
        unit = FindObjectOfType<Unit>();
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
                abilityTut.SetActive(true);
            }

            if (isBossEntrance)
            {
                unit._animator.SetTrigger("Entrance");
                bossHPBar.SetActive(true);

                playerController._moveSpeed = 1f;
            }

            if (isBossPreFight)
            {
                playerController._moveSpeed = 5f;
            }
        }
    }
}
