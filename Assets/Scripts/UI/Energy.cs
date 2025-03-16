using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public Sprite fullEnergy, emptyEnergy;
    Image energyImage;

    private void Awake()
    {
        energyImage = GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void SetEnergyImage(EnergyStatus status)
    {
        switch (status)
        {
            case EnergyStatus.Empty:
                energyImage.sprite = emptyEnergy;
                break;
            case EnergyStatus.Full:
                energyImage.sprite = fullEnergy;
                break;
        }
    }
}

public enum EnergyStatus
{
    Empty = 0,
    Full = 1,
}
