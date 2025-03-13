using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [Header ("Basics")]
    public int health = 150;

    [Header("Data Track")]
    [SerializeField] private int itemId;

    [Header("HitFlash")]
    [SerializeField] private HitFlash flashEffect;

    private void OnEnable()
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

    void Start()
    {
        flashEffect = GetComponent<HitFlash>();

        if (SpawnData.Instance != null && SpawnData.Instance.items.Find(e => e.id == itemId) == null)
        {
            SpawnData.Instance.AddItem(gameObject, itemId, false);
        }
    }

    private void Update()
    {

    }

    private void HandleSwing(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            SpawnData.Instance.items.Find(e => e.id == itemId).dead = true;

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Swing-A"))
        {
            HandleSwing(50);

            flashEffect.Flash();
        }
    }
}
