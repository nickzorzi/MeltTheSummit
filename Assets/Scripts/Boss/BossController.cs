using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    public static event Action OnBossDamaged;

    [Header("Basics")]
    public int health;
    public int maxHealth;
    private Rigidbody2D _rb;
    [SerializeField] private GameObject player;
    [SerializeField] private float knockbackForce;
    public bool isKnockback = false;

    [Header("Combat Checks")]
    private bool hasHesitate = false;
    private bool hasTakenDamageThisSwing = false;
    [SerializeField] private bool canAttack = true;
    public bool hasLineOfSight = false;
    public bool hasRange = false;

    [Header("Firing")]
    [SerializeField] private Transform gunPivot;

    [Header("Swinging")]
    [SerializeField] private float nextMeleeTime;
    [SerializeField] private float meleeTime;

    [Header("Phases")]
    [SerializeField] private bool isPhaseMelee = false;
    [SerializeField] private bool isPhaseBoomerang = true;
    [SerializeField] private bool isPhaseHands = false;
    [SerializeField] private bool isPhaseShock = false;
    [SerializeField] private bool isPhaseFreeze = false;

    [Header("Boomerang")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float nextFireTime;
    [SerializeField] private float fireTime;

    [Header("Freeze Orb")]
    [SerializeField] private GameObject freezeOrb;
    [SerializeField] private float nextDropTime;
    [SerializeField] private float dropTime;

    [Header("Shock")]
    [SerializeField] private List<GameObject> puddles = new List<GameObject>();
    [SerializeField] private GameObject shock;
    [SerializeField] private bool canFireShock = true;

    [Header("Ice Hand")]
    [SerializeField] private GameObject hand;
    [SerializeField] private bool canFireHands = true;

    [Header("Nav")]
    [SerializeField] private LayerMask colliders;
    [SerializeField] private float distance;
    [SerializeField] private float distanceRange;
    private Unit unit;

    [Header("HitFlash")]
    [SerializeField] private HitFlash flashEffect;

    [Header("Dialogue")]
    [SerializeField] private bool isPreFight;
    [SerializeField] private GameObject preFightDialogue;

    [Header("Audio")]
    [SerializeField] private AudioClip hitDMG;
    [SerializeField] private AudioClip knockbackFX;
    [SerializeField] private AudioClip deathFX;
    [SerializeField] private AudioClip shootFX;
    [SerializeField] private AudioClip swingFX;
    [SerializeField] private AudioClip dropFX;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
        flashEffect = GetComponent<HitFlash>();
    }

    private void OnEnable()
    {
        OnBossDamaged?.Invoke();
    }

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;

        //fireTime = nextFireTime;
    }

    private void FixedUpdate()
    {
        checkForPlayer();

        FindPuddles();

    }

    private void Update()
    {
        if (unit._animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Entrance") || unit._animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Statue"))
        {
            return;
        }

        if (isPreFight)
        {
            preFightDialogue.SetActive(true);
            isPreFight = false;
        }

        HandleBattleStates();

        if (canAttack && !hasHesitate && isPhaseFreeze)
        {
            if (dropTime <= 0)
            {
                //unit._animator.SetTrigger("Throw");

                Instantiate(freezeOrb, gunPivot.transform.position, Quaternion.identity);

                SoundFXManager.instance.PlaySoundClip(dropFX, transform, 1f);

                dropTime = nextDropTime;
            }
            else
            {
                dropTime -= Time.deltaTime;
            }
        }

        if (canAttack && isPhaseShock && canFireShock)
        {
            StartCoroutine(FireShock(shock, 3));
        }

        if (canAttack && isPhaseHands && canFireHands)
        {
            StartCoroutine(FireHands(hand, 5, 4));
        }

        if (canAttack && isPhaseBoomerang)
        {
            if (fireTime <= 0)
            {
                unit._animator.SetTrigger("Throw");

                Instantiate(projectile, gunPivot.transform.position, Quaternion.identity);

                SoundFXManager.instance.PlaySoundClip(shootFX, transform, 1f);

                fireTime = nextFireTime;
            }
            else
            {
                fireTime -= Time.deltaTime;
            }
        }

        if (hasRange && canAttack && !hasHesitate && isPhaseMelee)
        {
            if (meleeTime <= 0)
            {
                StartCoroutine(HandlePause(nextMeleeTime, 1));

                SoundFXManager.instance.PlaySoundClip(swingFX, transform, 1f);

                unit._animator.SetTrigger("Swing");

                meleeTime = nextMeleeTime;
            }
            else
            {
                meleeTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Swing-D") && !hasTakenDamageThisSwing)
        {
            HandleSwing(0, true);

            SoundFXManager.instance.PlaySoundClip(knockbackFX, transform, 1f);
        }
        else if (collider.CompareTag("Swing-A") && !hasTakenDamageThisSwing)
        {
            HandleSwing(1, false);

            SoundFXManager.instance.PlaySoundClip(hitDMG, transform, 1f);

            flashEffect.Flash();
        }
    }

    void FindPuddles()
    {
        puddles.Clear();

        GameObject[] puddleObjects = GameObject.FindGameObjectsWithTag("Puddle");

        puddles.AddRange(puddleObjects);
    }

    public IEnumerator FireShock(GameObject prefab, int delay)
    {
        canFireShock = false;

        if (puddles.Count > 0)
        {
            int randomIndex = Random.Range(0, puddles.Count);
            Transform puddleTransform = puddles[randomIndex].transform;

            Instantiate(prefab, puddleTransform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(delay);

        canFireShock = true;
    }

    private IEnumerator FireHands(GameObject prefab, int handCount, float firingCoolDown)
    {
        canFireHands = false;

        for (int i = 0; i < handCount; i++)
        {
            yield return new WaitForSeconds(2f);
            GameObject temp = Instantiate(prefab, new Vector2(0, player.transform.position.y), Quaternion.identity);
        }

        yield return new WaitForSeconds(firingCoolDown);

        canFireHands = true;
    }

    private void HandleSwing(int damage, bool isKnockback)
    {
        health -= damage;

        OnBossDamaged?.Invoke();

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

        Vector2 directionToPlayer = (Vector2)(transform.position - player.transform.position);
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
        if (health <= 5)
        {
            //isPhaseShock = false;
            isPhaseHands = true;
        }
        else if (health <= 10)
        {
            isPhaseShock = true;
            //isPhaseFreeze = false;

            isPhaseBoomerang = false;
            isPhaseMelee = true;
        }
        else if (health <= 15)
        {
            isPhaseFreeze = true;
        }
    }

    private void Die()
    {
        Destroy(gameObject);

        SoundFXManager.instance.PlaySoundClip(deathFX, transform, 1f);

        SceneManager.LoadScene("Victory");
    }
}
