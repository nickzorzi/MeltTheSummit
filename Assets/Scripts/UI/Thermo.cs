using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thermo : MonoBehaviour
{
    public Sprite fullThermo, threeForthsThermo, halfThermo, oneForthsThermo, emptyThermo;
    Image thermoImage;

    private void Awake()
    {
        thermoImage = GetComponent<Image>();
    }

    public void SetThermoImage(ThermoStatus status)
    {
        switch (status)
        {
            case ThermoStatus.Empty:
                thermoImage.sprite = emptyThermo;
                break;
            case ThermoStatus.OneForths:
                thermoImage.sprite = oneForthsThermo;
                break;
            case ThermoStatus.Half:
                thermoImage.sprite = halfThermo;
                break;
            case ThermoStatus.ThreeForths:
                thermoImage.sprite = threeForthsThermo;
                break;
            case ThermoStatus.Full:
                thermoImage.sprite = fullThermo;
                break;
        }
    }
}

public enum ThermoStatus
{
    Empty = 0,
    OneForths = 1,
    Half = 2,
    ThreeForths = 3,
    Full = 4,
}
