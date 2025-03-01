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

    private void Start()
    {
        PlayerController.OnPlayerDamaged += HandleHealthPurchase;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDamaged -= HandleHealthPurchase;
    }

    private void HandleHealthPurchase()
    {
        Debug.Log("Player bought health");
    }

    private void Update()
    {
        if (InputManager.isInteractTriggered && (Collected.currencyValue >= price) && canPurchase)
        {
            switch (loadEffect)
            {
                case ItemEffects.Health:
                    if (playerController.health < playerController.maxHealth)
                    {
                        //playerController.health++;
                        Collected.currencyValue = Collected.currencyValue - price;
                        //playerController._healthUpdate = true;
                        playerController.GainHealth(1);
                    }
                    break;

                case ItemEffects.CoolCost:
                    if (playerController.coolCost == -1)
                    {
                        playerController.coolCost = -2;
                        Collected.currencyValue = Collected.currencyValue - price;
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
        if (infoUI != null)
        {
            infoUI.SetActive(false);
        }

        canPurchase = false;
    }
}
