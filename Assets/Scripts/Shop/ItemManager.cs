using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemEffects
    {
        Health,
        CoolCost
    }

    public int price;

    [SerializeField] private ItemEffects loadEffect;
    [SerializeField] private GameObject infoUI;
    [SerializeField] private bool canPurchase = false;

    PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

    }

    private void Update()
    {
        if (InputManager.isInteractTriggered && (playerController.currency >= price) && canPurchase)
        {
            switch (loadEffect)
            {
                case ItemEffects.Health:
                    if (playerController.health < playerController.maxHealth)
                    {
                        playerController.health++;
                        playerController.currency = playerController.currency - price;
                    }
                    break;

                case ItemEffects.CoolCost:
                    if (playerController.coolCost == -1)
                    {
                        playerController.coolCost = -2;
                        playerController.currency = playerController.currency - price;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            infoUI.SetActive(true);

            canPurchase = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        infoUI.SetActive(false);

        canPurchase = false;
    }
}
