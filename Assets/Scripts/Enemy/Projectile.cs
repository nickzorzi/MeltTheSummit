using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D projectileRB;

    void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");


        Vector2 offsetTargetPosition = new Vector2(target.transform.position.x, target.transform.position.y + 0.75f);

        Vector2 moveDir = (offsetTargetPosition - (Vector2)transform.position).normalized * speed;
        projectileRB.velocity = new Vector2(moveDir.x, moveDir.y);
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

