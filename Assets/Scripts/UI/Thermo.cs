using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thermo : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public Sprite fullThermo, threeForthsThermo, halfThermo, oneForthsThermo, emptyThermo;
    Image thermoImage;

    private void Awake()
    {
        thermoImage = GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
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
