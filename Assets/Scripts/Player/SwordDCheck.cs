using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDCheck : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Projectile") && playerController.temp >= 3)
        {
            playerController.temp = playerController.temp - 3;
        }
    }
}
