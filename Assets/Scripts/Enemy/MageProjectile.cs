using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    GameObject target;
    public float speed;

    [SerializeField] private GameObject returnProjectile;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        Destroy(this.gameObject, 6);
    }

    private void Update()
    {
        Vector2 offsetTargetPosition = new Vector2(target.transform.position.x, target.transform.position.y + 0.75f);

        transform.position = Vector2.MoveTowards(transform.position, offsetTargetPosition, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("Wall") || hitInfo.CompareTag("Swing-A"))
        {
            Destroy(gameObject);
        }

        if (hitInfo.CompareTag("Swing-D"))
        {
            Instantiate(returnProjectile, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
