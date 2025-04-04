using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public Sprite fullHeart, threeForthsHeart, halfHeart, oneForthsHeart, emptyHeart;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.OneForths:
                heartImage.sprite = oneForthsHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.ThreeForths:
                heartImage.sprite = threeForthsHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}

public enum HeartStatus
{
    Empty = 0,
    OneForths = 1,
    Half = 2,
    ThreeForths = 3,
    Full = 4,
}
