using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemEffects
    {
        Health,
        CoolCost,
        HeatResistance
    }

    public int price;

    [SerializeField] private ItemEffects loadEffect;
    [SerializeField] private GameObject infoUI;
    [SerializeField] private bool canPurchase = false;

    [SerializeField] private GameObject item;

    PlayerController playerController;

    [Header("Data Track")]
    [SerializeField] private int itemId;

    [Header("SFX")]
    [SerializeField] private AudioClip purchase;

    private void OnEnable()
    {
        if (loadEffect == ItemEffects.CoolCost || loadEffect == ItemEffects.HeatResistance)
        {
            if (SpawnData.Instance != null && SpawnData.Instance.items != null)
            {
                var item = SpawnData.Instance.items.Find(e => e.id == itemId);
                if (item != null && item.dead == true)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

    }

    private void Start()
    {
        PlayerController.OnPlayerDamaged += HandleHealthPurchase;

        if (loadEffect == ItemEffects.CoolCost || loadEffect == ItemEffects.HeatResistance)
        {
            if (SpawnData.Instance != null && SpawnData.Instance.items.Find(e => e.id == itemId) == null)
            {
                SpawnData.Instance.AddItem(gameObject, itemId, false);
            }
        }
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
                        SpawnData.Instance.items.Find(e => e.id == itemId).dead = true;
                        Destroy(item);
                    }
                    break;

                case ItemEffects.HeatResistance:
                    if (playerController.heatCost == 1)
                    {
                        playerController.heatCost = 0.75f;
                        Collected.currencyValue = Collected.currencyValue - price;
                        SpawnData.Instance.items.Find(e => e.id == itemId).dead = true;
                        Destroy(item);
                    }
                    break;

                default:
                    break;
            }

            SoundFXManager.instance.PlaySoundClip(purchase, transform, 1f);
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
