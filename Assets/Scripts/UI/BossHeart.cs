using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHeart : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public Sprite fullBossHeart, halfBossHeart, emptyBossHeart;
    Image bossHeartImage;

    private void Awake()
    {
        bossHeartImage = GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void SetBossHeartImage(BossHeartStatus status)
    {
        switch (status)
        {
            case BossHeartStatus.Empty:
                bossHeartImage.sprite = emptyBossHeart;
                break;
            case BossHeartStatus.Half:
                bossHeartImage.sprite = halfBossHeart;
                break;
            case BossHeartStatus.Full:
                bossHeartImage.sprite = fullBossHeart;
                break;
        }
    }
}

public enum BossHeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2,
}
