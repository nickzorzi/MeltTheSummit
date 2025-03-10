using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFlower : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private GameObject flower;

    [Header("Data Track")]
    [SerializeField] private int enemyId;

    private void OnEnable()
    {
        if (SpawnData.Instance != null && SpawnData.Instance.enemies != null)
        {
            var enemy = SpawnData.Instance.enemies.Find(e => e.id == enemyId);
            if (enemy != null && enemy.dead == true)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        if (SpawnData.Instance != null && SpawnData.Instance.enemies.Find(e => e.id == enemyId) == null)
        {
            SpawnData.Instance.AddEnemy(gameObject, enemyId, false);
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
            Destroy(gameObject);
        }

        if (hitInfo.CompareTag("Swing-A"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SpawnData.Instance.enemies.Find(e => e.id == enemyId).dead = true;
    }
}
