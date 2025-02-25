using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int health;
    public bool isKnockback = false;

    private Rigidbody2D _rb;
    [SerializeField] private Transform player;
    [SerializeField] private float knockbackForce;

    private bool hasTakenDamageThisSwing = false;
    [SerializeField] private bool canAttack = false;

    [SerializeField] private float distance;
    [SerializeField] private float distanceRange;
    [SerializeField] private LayerMask colliders;
    public bool hasLineOfSight = false;
    public bool hasRange = false;

    private Unit unit;
    public bool _isAttacking = false;

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
    }

    private void FixedUpdate()
    {
        checkForPlayer();
    }

    private void Update()
    {
        HandleBattleStates();

        if (hasRange && !_isAttacking)
        {
            StartCoroutine(SwingAttack(2f));
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
            Debug.Log("The player is close to the boss!");
            hasRange = true;
        }
        else
        {
            Debug.Log("The player is too far from the boss.");
            hasRange = false;
        }
    }

    private IEnumerator SwingAttack(float swingTime)
    {
        yield return new WaitForSeconds(0.5f);
        unit._animator.SetTrigger("Swing");
        _isAttacking = true;
        unit.speed = 0f;
        yield return new WaitForSeconds(swingTime);
        _isAttacking = false;
        unit.speed = 1f;
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
