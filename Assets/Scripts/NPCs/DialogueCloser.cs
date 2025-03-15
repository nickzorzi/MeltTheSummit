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
    public PlayerController player;

    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if (InputManager.isInteractTriggered)
        {
            Time.timeScale = 1;

            dialogue.SetActive(false);

            if (isSilver)
            {
                Collected.currencyValue = Collected.currencyValue + dropAmount;
            }

            if (isFlower)
            {
                Collected.flowerValue = Collected.flowerValue + dropAmount;
            }

            if (!isAbility)
            {
                player._canAbility = true;
            }
        }
    }
}
