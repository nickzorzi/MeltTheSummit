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

    [Header("Audio")]
    [SerializeField] private AudioClip shootFX;

    void Start()
    {
        //fireTime = nextFireTime;
    }

    void Update()
    {
        if (fireTime <= 0)
        {
            Instantiate(turretProjectile, gunPivot.transform.position, Quaternion.identity);

            SoundFXManager.instance.PlaySoundClip(shootFX, transform, 1f);

            fireTime = nextFireTime;
        }
        else
        {
            fireTime -= Time.deltaTime;
        }
    }
}
