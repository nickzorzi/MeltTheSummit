using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    GameObject target;
    GameObject boss;
    public float speed;
    Rigidbody2D projectileRB;

    Vector2 initialPosition;
    Vector2 targetPosition;

    bool returning = false;
    float returnTime = 1.5f;
    float timeElapsed = 0f;

    void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");

        targetPosition = new Vector2(target.transform.position.x, target.transform.position.y + 0.75f);

        MoveToTarget(targetPosition);
        Destroy(this.gameObject, 3);
    }

    void Update()
    {
        initialPosition = new Vector2(boss.transform.position.x, boss.transform.position.y + 0.75f);

        if (!returning && timeElapsed >= returnTime)
        {
            returning = true;
        }

        if (returning)
        {
            MoveToTarget(initialPosition);
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
    }

    void MoveToTarget(Vector2 destination)
    {
        Vector2 moveDir = (destination - (Vector2)transform.position).normalized * speed;
        projectileRB.velocity = new Vector2(moveDir.x, moveDir.y);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("Swing-A") || hitInfo.CompareTag("Swing-D") || hitInfo.CompareTag("Wall") || returning && hitInfo.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
