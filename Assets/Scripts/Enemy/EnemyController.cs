using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 100;
    public bool isKnockback = false;

    private Rigidbody2D _rb;
    [SerializeField] private Transform player;
    [SerializeField] private float knockbackForce = 5f;

    private bool hasTakenDamageThisSwing = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
       
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
        Vector2 directionToPlayer = (Vector2)(transform.position - player.position);
        Vector2 knockbackDirection = directionToPlayer.normalized;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackCooldown);

        isKnockback = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
