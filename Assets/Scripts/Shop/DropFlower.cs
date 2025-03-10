using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFlower : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private GameObject flower;

    [Header("Data Track")]
    [SerializeField] private int itemId;

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
        if (SpawnData.Instance != null && SpawnData.Instance.items.Find(e => e.id == itemId) == null)
        {
            SpawnData.Instance.AddItem(gameObject, itemId, false);
        }
    }

    private void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Swing-D"))
        {
            Instantiate(flower, transform.position, Quaternion.identity);

            SpawnData.Instance.items.Find(e => e.id == itemId).dead = true;

            Destroy(gameObject);
        }

        if (hitInfo.CompareTag("Swing-A"))
        {
            SpawnData.Instance.items.Find(e => e.id == itemId).dead = true;

            Destroy(gameObject);
        }
    }
}
