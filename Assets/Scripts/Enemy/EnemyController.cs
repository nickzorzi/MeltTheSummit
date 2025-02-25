using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 100;
    public bool isKnockback = false;

    private Rigidbody2D _rb;
    [SerializeField] private Transform player;
    [SerializeField] private float knockbackForce = 5f;

    private bool hasTakenDamageThisSwing = false;
    public bool typeShooter = true;
    [SerializeField] private bool canShoot = true;

    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask colliders;
    public bool hasLineOfSight = false;

    public GameObject projectile;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private float nextFireTime;
    public float fireRate = 1f;

    [SerializeField] private HitFlash flashEffect;

    public GameObject silver;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        flashEffect = GetComponent<HitFlash>();
    }

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void FixedUpdate()
    {
        checkForPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Swing-D") && !hasTakenDamageThisSwing)
        {
            HandleSwing(0, true);
        }
        else if (collider.CompareTag("Swing-A") && !hasTakenDamageThisSwing)
        {
            HandleSwing(50, false);

            flashEffect.Flash();
        }
    }

    private void HandleSwing(int damage, bool isKnockback)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        if (isKnockback)
        {
            this.isKnockback = true;
            StartCoroutine(swingKnockback(1));
        }

        hasTakenDamageThisSwing = true;

        StartCoroutine(ResetDamageFlag());
    }

    private IEnumerator ResetDamageFlag()
    {
        yield return new WaitForSeconds(0.1f);
        hasTakenDamageThisSwing = false;
    }

    private IEnumerator swingKnockback(int knockbackCooldown)
    {
        canShoot = false;

        Vector2 directionToPlayer = (Vector2)(transform.position - player.position);
        Vector2 knockbackDirection = directionToPlayer.normalized;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackCooldown);

        isKnockback = false;
        canShoot = true;
    }

    private void checkForPlayer ()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, distance, ~colliders);

        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player") && ray.collider.isTrigger;

            if (hasLineOfSight)
            {
                Debug.DrawLine(transform.position, ray.point, Color.green);

                if (typeShooter && canShoot)
                {
                    HandleShooting();
                }
            }
            else
            {
                Debug.DrawLine(transform.position, ray.point, Color.red);
            }

            Debug.Log(ray.collider.gameObject.name);
        }
    }

    private void HandleShooting()
    {
        if (nextFireTime < Time.time)
        {
            Instantiate(projectile, gunPivot.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Die()
    {
        Instantiate(silver, gunPivot.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
