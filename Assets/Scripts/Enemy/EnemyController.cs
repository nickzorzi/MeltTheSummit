using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Basics")]
    private Rigidbody2D _rb;
    private Unit unit;
    public int health = 100;
    [SerializeField] private float knockbackForce;
    [SerializeField] private Transform player;
    public bool isKnockback = false;

    [Header("Combat Checks")]
    private bool hasHesitate = false;
    private bool hasTakenDamageThisSwing = false;
    [SerializeField] private bool canAttack = true;

    [Header("Shooter")]
    [SerializeField] private bool typeShooter = false;
    public GameObject projectile;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private float nextFireTime;
    public float fireRate;

    [Header("Meele")]
    [SerializeField] private bool typeMelee = false;
    [SerializeField] private bool hasRange = false;
    [SerializeField] private float dashForce;
    [SerializeField] private float meleeRange;

    [Header("Nav")]
    [SerializeField] private float distance;
    [SerializeField] private LayerMask colliders;
    public bool hasLineOfSight = false;

    [Header("HitFlash")]
    [SerializeField] private HitFlash flashEffect;

    [Header("Drops")]
    public GameObject silver;

    [Header("Data Track")]
    [SerializeField] private int enemyId;

    private void OnEnable()
    {
        if (SpawnData.Instance != null && SpawnData.Instance.enemies.Find(e => e.id == enemyId).dead == true)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        flashEffect = GetComponent<HitFlash>();
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;

        if (SpawnData.Instance != null && SpawnData.Instance.enemies.Find(e => e.id == enemyId) == null)
        {
            SpawnData.Instance.AddEnemy(gameObject, enemyId, false);
        }
    }

    private void FixedUpdate()
    {
        checkForPlayer();

        if (hasRange && canAttack && typeMelee)
        {
            StartCoroutine(HandleMelee(1.5f, 2));
        }
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

    private void HandleSwing(int damage, bool hasKnockback)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        if (hasKnockback)
        {
            StartCoroutine(swingKnockback(1));
        }

        hasTakenDamageThisSwing = true;

        StartCoroutine(ResetDamageFlag());

        if (!hasHesitate)
        {
            StartCoroutine(HesitateAfterHit());
        }
    }

    private IEnumerator ResetDamageFlag()
    {
        yield return new WaitForSeconds(0.1f);
        hasTakenDamageThisSwing = false;
    }

    private IEnumerator HesitateAfterHit()
    {
        hasHesitate = true;
        yield return new WaitForSeconds(1f);
        hasHesitate = false;
    }

    private IEnumerator swingKnockback(int knockbackCooldown)
    {
        isKnockback = true;
        canAttack = false;

        Vector2 directionToPlayer = (Vector2)(transform.position - player.position);
        Vector2 knockbackDirection = directionToPlayer.normalized;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackCooldown);

        isKnockback = false;
        canAttack = true;
    }

    private void checkForPlayer ()
    {
        if (isKnockback || !canAttack)
        {
            return;
        }

        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, distance, ~colliders);

        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player") && ray.collider.isTrigger;

            if (hasLineOfSight)
            {
                Debug.DrawLine(transform.position, ray.point, Color.green);

                if (typeShooter && canAttack)
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

        float distanceBetween = Vector3.Distance(transform.position, player.transform.position);

        if (distanceBetween <= meleeRange)
        {
            hasRange = true;
        }
        else
        {
            hasRange = false;
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

    IEnumerator HandleMelee(float cooldown, float speed)
    {
        if (!canAttack || isKnockback)
        {
            yield break;
        }

        canAttack = false;

        unit.speed = 0f;

        yield return new WaitForSeconds(0.5f);

        unit._animator.SetTrigger("Swing");

        Vector2 directionToPlayer = (Vector2)(transform.position - player.position);
        Vector2 dashDirection = -directionToPlayer.normalized;
        _rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(cooldown);

        unit.speed = speed;

        canAttack = true;
    }

    private void Die()
    {
        Instantiate(silver, gunPivot.transform.position, Quaternion.identity);

        SpawnData.Instance.enemies.Find(e => e.id == enemyId).dead = true;

        Destroy(gameObject);
    }

}
