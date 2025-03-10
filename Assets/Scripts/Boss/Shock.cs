using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Silver"))
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
    }
}
