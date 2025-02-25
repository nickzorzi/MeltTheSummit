using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    public PlayerController playerController;

    void Update()
    {
        if (currencyText != null && playerController != null)
        {
            currencyText.text = "Currency: " + playerController.currency.ToString();
        }
    }
}