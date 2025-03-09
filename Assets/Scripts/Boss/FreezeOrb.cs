using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeOrb : MonoBehaviour
{
    [Header("Basic")]
    public int health = 100;
    private Rigidbody2D _rb;
    [SerializeField] private float knockbackForce;
    [SerializeField] private GameObject player;
    public bool isKnockback = false;

    [Header("Combat Checks")]
    private bool hasTakenDamageThisSwing = false;

    [Header("Puddle")]
    [SerializeField] private GameObject puddle;

    [Header("HitFlash")]
    [SerializeField] private HitFlash flashEffect;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        flashEffect = GetComponent<HitFlash>();
        player = GameObject.FindGameObjectWithTag("Player");

        //Destroy(this.gameObject, 10);

        StartCoroutine(Melt(10));
    }

    private void Update()
    {
        
    }

    private void HandleSwing(int damage, bool hasKnockback)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (hasKnockback)
        {
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
        isKnockback = true;

        Vector2 directionToPlayer = (Vector2)(transform.position - player.transform.position);
        Vector2 knockbackDirection = directionToPlayer.normalized;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackCooldown);

        isKnockback = false;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Swing-A"))
        {
            HandleSwing(50, false);

            flashEffect.Flash();
        }

        if (hitInfo.CompareTag("Swing-D"))
        {
            HandleSwing(0, true);
        }
    }

    IEnumerator Melt(int time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(puddle, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
