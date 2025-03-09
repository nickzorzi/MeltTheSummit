using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] private GameObject turretProjectile;
    [SerializeField] private Transform gunPivot;

    [Header("Combat Checks")]
    [SerializeField] private float nextFireTime;
    [SerializeField] private float fireTime;

    void Start()
    {
        //fireTime = nextFireTime;
    }

    void Update()
    {
        if (fireTime <= 0)
        {
            Instantiate(turretProjectile, gunPivot.transform.position, Quaternion.identity);
            fireTime = nextFireTime;
        }
        else
        {
            fireTime -= Time.deltaTime;
        }
    }
}
