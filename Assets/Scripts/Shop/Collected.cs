using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collected : MonoBehaviour
{
    public static int currencyValue = 0;
    public TextMeshProUGUI currencyText;

    private void Update()
    {
        if (currencyText != null)
        {
            currencyText.text = "Currency: " + currencyValue.ToString();
        }
    }

    private void OnEnable()
    {
        if (PlayerData.Instance != null)
        {
            currencyValue = PlayerData.Instance.currency;
        }
    }

    private void OnDisable()
    {
        PlayerData.Instance.currency = currencyValue;
    }
}
