using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collected : MonoBehaviour
{
    [Header("Silver")]
    public static int currencyValue = 0;
    public TextMeshProUGUI currencyText;

    [Header("Flower")]
    public static int flowerValue = 0;
    public TextMeshProUGUI flowerText;

    private void Update()
    {
        if (currencyValue <=0)
        {
            currencyValue = 0;
        }

        if (flowerValue <=0)
        {
            flowerValue = 0;
        }

        if (currencyText != null)
        {
            currencyText.text = "Silver: " + currencyValue.ToString();
        }

        if (flowerText != null)
        {
            flowerText.text = "Flower: " + flowerValue.ToString();
        }
    }

    private void OnEnable()
    {
        if (PlayerData.Instance != null)
        {
            currencyValue = PlayerData.Instance.currency;

            flowerValue = PlayerData.Instance.flowers;
        }
    }

    private void OnDisable()
    {
        PlayerData.Instance.currency = currencyValue;

        PlayerData.Instance.flowers = flowerValue;
    }
}
