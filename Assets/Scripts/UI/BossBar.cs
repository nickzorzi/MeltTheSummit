using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBar : MonoBehaviour
{
    public GameObject bossHeartPrefab;
    public BossController bossController;
    List<BossHeart> bossHearts = new List<BossHeart>();

    private void OnEnable()
    {
        BossController.OnBossDamaged += DrawBossHearts;
    }

    private void OnDisable()
    {
        BossController.OnBossDamaged -= DrawBossHearts;
    }

    private void Start()
    {
        DrawBossHearts();
    }

    public void DrawBossHearts()
    {
        ClearBossHearts();

        float maxHealthRemainder = bossController.maxHealth % 2;
        int heartsToMake = (int)(bossController.maxHealth / 2 + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyBossHeart();
        }

        for (int i = 0; i < bossHearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(bossController.health - (i * 2), 0, 2);
            bossHearts[i].SetBossHeartImage((BossHeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyBossHeart()
    {
        GameObject newHeart = Instantiate(bossHeartPrefab);
        newHeart.transform.SetParent(transform);

        int insertIndex = bossHearts.Count + 1;
        newHeart.transform.SetSiblingIndex(insertIndex);

        BossHeart heartComponent = newHeart.GetComponent<BossHeart>();
        heartComponent.SetBossHeartImage(BossHeartStatus.Empty);
        bossHearts.Add(heartComponent);
    }

    public void ClearBossHearts()
    {
        foreach (Transform t in transform)
        {
            if (t.name == "Front" || t.name == "Back")
            {
                continue;
            }

            Destroy(t.gameObject);
        }
        bossHearts = new List<BossHeart>();
    }
}
