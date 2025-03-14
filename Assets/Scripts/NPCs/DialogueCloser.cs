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
    [SerializeField] private int dropAmount;

    void Start()
    {
        
    }

    void Update()
    {
        if (InputManager.isInteractTriggered)
        {
            dialogue.SetActive(false);

            if (isSilver)
            {
                Collected.currencyValue = Collected.currencyValue + dropAmount;
            }

            if (isFlower)
            {
                Collected.flowerValue = Collected.flowerValue + dropAmount;
            }
        }
    }
}
