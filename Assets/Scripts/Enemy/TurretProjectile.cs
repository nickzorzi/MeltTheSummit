using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    GameObject target;
    public float speedY;
    public float speedX;
    Rigidbody2D projectileRB;

    void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();

        projectileRB.velocity = new Vector2(speedX, speedY);

        Destroy(this.gameObject, 6);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("Swing-A") || hitInfo.CompareTag("Swing-D") || hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
