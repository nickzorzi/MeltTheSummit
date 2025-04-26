using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReturnProjectile : MonoBehaviour
{
    GameObject[] targets;
    GameObject enemy;
    public float speed;
    public float raycastDistance = 10f;
    private bool foundEnemy = false;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");

        Destroy(this.gameObject, 6);
    }

    private void Update()
    {
        if (!foundEnemy)
        {
            foreach (GameObject target in targets)
            {
                if (target != null)
                {
                    RaycastHit2D ray = Physics2D.Raycast(transform.position, target.transform.position - transform.position, raycastDistance);
                    if (ray.collider != null && ray.collider.CompareTag("Enemy"))
                    {
                        foundEnemy = true;
                        enemy = target;
                        break;
                    }
                }
            }
        }

        if (foundEnemy && enemy != null)
        {
            Vector2 offsetTargetPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 0.75f);
            transform.position = Vector2.MoveTowards(transform.position, offsetTargetPosition, speed * Time.deltaTime);
        }
        else if (foundEnemy && enemy == null)
        {
            Destroy(this.gameObject);
        }
    }

    private void OldCheck()
    {
        //if (target != null && target.activeInHierarchy)
       // {
            //Vector2 offsetTargetPosition = new Vector2(target.transform.position.x, target.transform.position.y + 0.75f);
            //transform.position = Vector2.MoveTowards(transform.position, offsetTargetPosition, speed * Time.deltaTime);

           // CheckEnemy();
      //  }
       // else
       // {
          //  target = null;
            //target = GameObject.FindGameObjectWithTag("Enemy");

           // if (target != null)
         //   {
         //       Destroy(gameObject);
        //    }
      //  }
    }

    private void CheckEnemy()
    {
       // RaycastHit2D ray = Physics2D.Raycast(transform.position, target.transform.position - transform.position, raycastDistance);

        //if (ray.collider.gameObject.CompareTag("Wall"))
        //{
            
        //}
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Enemy") || hitInfo.CompareTag("Boss") || hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
