using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [Header ("Basics")]
    public int health = 150;
    private Transform pos;

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

        pos = transform;

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

            StartCoroutine(ShakeOnHit());
        }

        if (hitInfo.CompareTag("Swing-D"))
        {
            StartCoroutine(ShakeOnHit());
        }
    }

    private IEnumerator ShakeOnHit()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i % 2 == 0)
            {
                transform.Translate(new Vector2(-0.05f, 0));
            }
            else
            {
                transform.Translate(new Vector2(0.05f, 0));
            }
            yield return new WaitForSeconds(0.1f);
        }

        transform.position = pos.position;
    }
}
