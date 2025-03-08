using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Basics")]
    public int health;
    private Rigidbody2D _rb;
    [SerializeField] private Transform player;
    [SerializeField] private float knockbackForce;
    public bool isKnockback = false;

    [Header("Combat Checks")]
    private bool hasHesitate = false;
    private bool hasTakenDamageThisSwing = false;
    [SerializeField] private bool canAttack = true;
    public bool hasLineOfSight = false;
    public bool hasRange = false;

    [Header("Firing")]
    [SerializeField] private float nextFireTime;
    [SerializeField] private float fireTime;

    [Header("Phases")]
    [SerializeField] private bool isPhaseMelee;

    [Header("Nav")]
    [SerializeField] private LayerMask colliders;
    [SerializeField] private float distance;
    [SerializeField] private float distanceRange;
    private Unit unit;

    [Header("HitFlash")]
    [SerializeField] private HitFlash flashEffect;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
        flashEffect = GetComponent<HitFlash>();
    }

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;

        //fireTime = nextFireTime;
    }

    private void FixedUpdate()
    {
        checkForPlayer();
    }

    private void Update()
    {
        HandleBattleStates();

        if (hasRange && canAttack && !hasHesitate)
        {
            if (fireTime <= 0)
            {
                StartCoroutine(HandlePause(nextFireTime, 1));

                unit._animator.SetTrigger("Swing");

                fireTime = nextFireTime;
            }
            else
            {
                fireTime -= Time.deltaTime;
            }
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

    private void HandleSwing(int damage, bool isKnockback)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        if (isKnockback)
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
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, distance, ~colliders);

        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player") && ray.collider.isTrigger;

            if (hasLineOfSight || hasRange)
            {
                Debug.DrawLine(transform.position, ray.point, Color.green);
            }
            else
            {
                Debug.DrawLine(transform.position, ray.point, Color.red);
            }
        }

        float distanceBetween = Vector3.Distance(transform.position, player.transform.position);

        if (distanceBetween <= distanceRange)
        {
            hasRange = true;
        }
        else
        {
            hasRange = false;
        }
    }

    IEnumerator HandlePause(float cooldown, float speed)
    {
        unit.speed = 0f;

        yield return new WaitForSeconds(cooldown);

        unit.speed = speed;
    }

    private void HandleBattleStates()
    {
        if (health == 750)
        {

        }
        else if (health == 500)
        {

        }
        else if (health == 250)
        {

        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
