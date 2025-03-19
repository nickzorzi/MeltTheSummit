using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnProjectile : MonoBehaviour
{
    GameObject target;
    public float speed;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Enemy");

        Destroy(this.gameObject, 6);
    }

    private void Update()
    {
        if (target != null)
        {
            Vector2 offsetTargetPosition = new Vector2(target.transform.position.x, target.transform.position.y + 0.75f);

            transform.position = Vector2.MoveTowards(transform.position, offsetTargetPosition, speed * Time.deltaTime);
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Enemy");
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Enemy") || hitInfo.CompareTag("Boss") || hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
