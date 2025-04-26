using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerThermo;
    public static event Action OnPlayerAbility;

    [Header ("Basics")]
    public float _moveSpeed = 5f;
    public float health = 12;
    public float maxHealth = 12;
    public float temp, maxTemp;
    public float heatCost;
    public int coolCost;
    [SerializeField] private int burn;

    [Header ("Checks")]
    public bool _canAttack = true;
    public bool _firstLoad = false;
    public bool _healthUpdate = false;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private bool _isTransformed = false;
    [SerializeField] private bool _isBurning = false;
    [SerializeField] private bool _died = false;
    public bool _inDialogue = false;
    public bool _inPause = false;
    public bool _inBossRoom = false;

    private bool _inCoroutine = false;

    private Vector2 _movement;

    private Rigidbody2D _rb;
    private Animator _animator;

    [Header("Ice Puzzle")]
    public bool _canSlide = false;
    [SerializeField] private Vector2 _slideDirection = Vector2.zero;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform rayPoint;

    [Header("Ability")]
    public bool _hasAbility = false;
    public bool _canAbility = false;
    public float abilityCooldown;
    public float abilityMaxCooldown;
    //[SerializeField] private GameObject abilityUI;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    [Header("I-Frames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    public bool isInv = false;

    [Header("Burn Flash")]
    [SerializeField] private HitFlash flashEffect;

    [Header("Cool Flash")]
    [SerializeField] private HitFlash coolEffect;

    [Header("Handle Boss")]
    [SerializeField] private GameObject boss;
    [SerializeField] private float knockForce;

    [Header("Audio")]
    [SerializeField] private AudioClip dSwing;
    [SerializeField] private AudioClip aSwing;
    [SerializeField] private AudioClip transformFX;
    [SerializeField] private AudioClip hitDMG;
    [SerializeField] private AudioClip burnDMG;
    [SerializeField] private AudioClip abilityFX;
    [SerializeField] private AudioClip deathFX;
    [SerializeField] private AudioClip reviveFX;

    private CheckpointData checkpointData;
    private SpawnData spawnData;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<HitFlash>();
        boss = GameObject.FindGameObjectWithTag("Boss");

        checkpointData = CheckpointData.Instance;
        spawnData = SpawnData.Instance;
    }

    private void OnEnable()
    {
        if (_firstLoad)
        {
            _firstLoad = false;
            return;
        }
        else
        {
            _died = false;

            health = PlayerData.Instance.health;
            coolCost = PlayerData.Instance.coolCost;
            heatCost = PlayerData.Instance.heatCost;
            temp = PlayerData.Instance.temp;
            _isTransformed = PlayerData.Instance._isTransformed;
            _isBurning = PlayerData.Instance._isBurning;
            _hasAbility = PlayerData.Instance._hasAbility;
            abilityCooldown = PlayerData.Instance.abilityCooldown;
            _firstLoad = PlayerData.Instance._firstLoad;

            OnPlayerDamaged?.Invoke();

            _firstLoad = false;
        }
    }

    private void OnDisable()
    {
        if (_died)
        {
            RevertEnemies();

            if (_inBossRoom)
            {
                MusicManager.instance.SetMusicTrack(9);
            }

            return;
        }

        PlayerData.Instance.health = health;
        PlayerData.Instance.coolCost = coolCost;
        PlayerData.Instance.heatCost = heatCost;
        PlayerData.Instance.temp = temp;
        PlayerData.Instance._isTransformed = _isTransformed;
        PlayerData.Instance._isBurning = _isBurning;
        PlayerData.Instance._hasAbility = _hasAbility;
        PlayerData.Instance.abilityCooldown = abilityCooldown;
        PlayerData.Instance._firstLoad = _firstLoad;
    }

    private void Start()
    {
        BackUpEnemies();
    }

    private void Update()
    {
        if (_inDialogue || _inPause)
        {
            return;
        }

        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        if (_canSlide)
        {
            if (_slideDirection == Vector2.zero && _movement != Vector2.zero)
            {
                if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
                {
                    _slideDirection = new Vector2(Mathf.Sign(_movement.x), 0);
                }
                else
                {
                    _slideDirection = new Vector2(0, Mathf.Sign(_movement.y));
                }
            }

            if (_slideDirection != Vector2.zero)
            {
                _moveSpeed = 8;
                _rb.velocity = _slideDirection * _moveSpeed;

                if (IsCollidingWithWall())
                {
                    _rb.velocity = Vector2.zero;
                    _slideDirection = Vector2.zero;
                    _moveSpeed = 5;

                    _rb.velocity = _movement * _moveSpeed;
                }
            }
        }
        else
        {
            _rb.velocity = _movement * _moveSpeed;

            _slideDirection = Vector2.zero;
        }

        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (!_isAttacking)
        {
            if (_movement != Vector2.zero)
            {
                _animator.SetFloat(_lastHorizontal, _movement.x);
                _animator.SetFloat(_lastVertical, _movement.y);
            }
        }

        HandleTemp();

        HandleHealth();

        if (_hasAbility)
        {
            HandleAbility();
        }

        if (!_canAttack)
        {
            return;
        }

        if (InputManager.isTransformTriggered && !_isTransformed && !_isAttacking)
        {
            _isTransformed = true;
            StartCoroutine(Transform("In"));
            SoundFXManager.instance.PlaySoundClip(transformFX, transform, 1f);
        }
        else if (InputManager.isTransformTriggered && _isTransformed && !_isAttacking)
        {
            _isTransformed = false;
            StartCoroutine(Transform("Out"));
            SoundFXManager.instance.PlaySoundClip(transformFX, transform, 1f);
        }

        if (InputManager.isSwingTriggered && !_isAttacking)
        {
            if (!_isTransformed)
            {
                StartCoroutine(Swing("D", 0.8f));
                SoundFXManager.instance.PlaySoundClip(dSwing, transform, 1f);
            }
            else if (_isTransformed)
            {
                StartCoroutine(Swing("A", 0.4f));
                SoundFXManager.instance.PlaySoundClip(aSwing, transform, 1f);
            }
        }

        if (InputManager.isAbilityTriggered && !_isAttacking && _canAbility && _hasAbility)
        {
            temp = 0;
            _isBurning = false;
            _canAbility = false;

            abilityCooldown = 0;

            SoundFXManager.instance.PlaySoundClip(abilityFX, transform, 1f);

            coolEffect.Flash();
        }
    }

    private bool IsCollidingWithWall()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, _slideDirection, 0.25f, wallLayer);
        //Collider2D hit = Physics2D.OverlapCircle(rayPoint.position, 0.41f, wallLayer);

        float boxWidth = 0.4f;
        float boxHeight = 0.2f;
        float offsetDistance = 0.3f;
        Vector2 boxCenter = (Vector2)rayPoint.position + _slideDirection * offsetDistance;
        Collider2D hit = Physics2D.OverlapBox(boxCenter, new Vector2(boxWidth, boxHeight), 0f, wallLayer);

        if (hit != null && hit.CompareTag("Wall")) //hit.collider != null && hit.collider.CompareTag("Wall)"
        {
            return true;
        }

        return false;
    }

    private IEnumerator Invulnerability()
    {
        isInv = true;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 1, 1, 0.25f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        isInv = false;
    }

    IEnumerator Swing(string mode, float time)
    {
        _animator.SetTrigger("Swing-" + mode);
        _isAttacking = true;
        _moveSpeed = 0f;
        yield return new WaitForSeconds(time);
        _isAttacking = false;
        _moveSpeed = 5f;
    }

    IEnumerator Transform(string mode)
    {
        _animator.SetTrigger("Transform-" + mode);
        _isAttacking = true;
        _moveSpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        _moveSpeed = 5f;
    }

    private void HandleHealth()
    {
        if (_isBurning && !_inCoroutine)
        {
            StartCoroutine(Burn(burn));
        }
        
        if (health <= 0)
        {
            Die();
        }
        else if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    private void HandleTemp()
    {
        if (_isTransformed)
        {
            StartCoroutine(ChangeTemp(heatCost, 0.1f));

            if (temp >= maxTemp)
            {
                temp = maxTemp;
                _isBurning = true;
            }
        }
        else
        {
            StartCoroutine(ChangeTemp(coolCost, 0.1f));
            _isBurning = false;

            if (temp <= 0)
            {
                temp = 0;
            }
        }
    }

    private void HandleAbility()
    {
        if (_hasAbility)
        {
            if (abilityCooldown >= 16)
            {
                _canAbility = true;

                abilityCooldown = abilityMaxCooldown;
            }

            if (!_canAbility)
            {
                abilityCooldown += Time.deltaTime;
            }

            OnPlayerAbility?.Invoke();
        }
    }

    IEnumerator ChangeTemp(float amount, float tickSpeed)
    {
        yield return new WaitForSeconds(tickSpeed);

        temp += amount * Time.deltaTime;
        temp = Mathf.Clamp(temp, 0, maxTemp);

        if (temp == 0)
        {
            yield return null;
        }
        else
        {
            OnPlayerThermo?.Invoke();
        }
    }

    IEnumerator Burn(int amount)
    {
        _inCoroutine = true;

        TakeDamage(amount);

        SoundFXManager.instance.PlaySoundClip(burnDMG, transform, 1f);

        flashEffect.Flash();

        yield return new WaitForSeconds(1);

        _inCoroutine = false;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Projectile") || hitInfo.CompareTag("MageProjectile"))
        {
            if (!isInv)
            {
                TakeDamage(1);

                SoundFXManager.instance.PlaySoundClip(hitDMG, transform, 1f);

                StartCoroutine(Invulnerability());
            }
        }

        if (hitInfo.CompareTag("Ice"))
        {
            _canSlide = true;
        }

        if (hitInfo.CompareTag("BossProjectile") || hitInfo.CompareTag("Shock"))
        {
            if (!isInv)
            {
                TakeDamage(2);

                SoundFXManager.instance.PlaySoundClip(hitDMG, transform, 1f);

                StartCoroutine(Invulnerability());
            }
        }

        if (hitInfo.CompareTag("Shock"))
        {
            _isTransformed = true;
            temp = maxTemp;
            OnPlayerThermo?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Ice"))
        {
            _canSlide = false;

            _moveSpeed = 5;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        CinemachineShake.Instance.ShakeCamera(3f, .1f);

        OnPlayerDamaged?.Invoke();
    }

    public void GainHealth(float amount)
    {
        health += amount;
        OnPlayerDamaged?.Invoke();
    }
    
    private void Die()
    {
        //SceneManager.LoadScene("Tutorial");

        if (Collected.flowerValue <= 0)
        {
            Collected.currencyValue = 0;
            Collected.flowerValue = 0;
            SpawnData.Instance.enemies.Clear();
            SpawnData.Instance.items.Clear();
            SpawnData.Instance.npcs.Clear();

            SceneManager.LoadScene("GameOver");

            //SoundFXManager.instance.PlaySoundClip(deathFX, transform, 1f);
        }
        else
        {
            Collected.flowerValue = Collected.flowerValue - 1;

            _died = true;

            SoundFXManager.instance.PlaySoundClip(reviveFX, transform, 1f);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void BackUpEnemies()
    {
        if (spawnData == null || checkpointData.checkpointEnemies == null || spawnData == null)
        {
            return;
        }

        checkpointData.checkpointEnemies.Clear();

        foreach (var spawnEnemy in spawnData.GetEnemies())
        {
            checkpointData.AddEnemy(spawnEnemy.enemy, spawnEnemy.id, spawnEnemy.dead);
        }
    }

    private void RevertEnemies()
    {
        spawnData.enemies.Clear();

        foreach (var checkpointEnemy in checkpointData.GetEnemies())
        {
            spawnData.AddEnemy(checkpointEnemy.enemy, checkpointEnemy.id, checkpointEnemy.dead);
        }
    }
}
