using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class DemonEnemy : MonoBehaviour, IDeathHandler
{
    public float moveSpeed = 4f;
    public float maxSpeed = 4f;
    public float moveStopSpeed = 0.6f;
    public float knockbackForce = 10f;
    private float lastAttackEndTime = -Mathf.Infinity;
    public float flipCooldownAfterAttack = 0.3f;


    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    public bool _HasTarget = false;
    private bool isAttacking = false;

    private Transform playerTransform;

    [Header("Fireball Spawn Settings")]
    public GameObject fireballPrefab;
    public int fireballCount = 5;
    public float fireballSpacing = 1f;
    public float fireballHeightOffset = 0.1f;
    public float fireballSpawnInterval = 6f;

    private float fireballSpawnTimer;

    [Header("Firewave Spawn Settings")]
    public GameObject firewavePrefab;
    public int firewaveCountPerSide = 5;
    public float firewaveSpacing = 1f;
    public float firewaveDelay = 0.1f;

    public void SpawnFirewaveLine()
    {
        StartCoroutine(FirewaveSpreadCoroutine());
    }

    private IEnumerator FirewaveSpreadCoroutine()
    {
        Vector2 origin = (Vector2)transform.position + walkDirectionVector * 5f; // przesuniêcie w kierunku, w którym demon patrzy
        float verticalOffset = 1f;
        float maxSpawnY = -7.5f; // Maksymalna wysokoœæ, powy¿ej której nie respimy fal

        for (int i = 0; i < firewaveCountPerSide; i++)
        {
            Vector2 leftCheck = origin + Vector2.left * firewaveSpacing * i;
            RaycastHit2D leftHit = Physics2D.Raycast(leftCheck, Vector2.down, 10f, LayerMask.GetMask("Ground"));

            // Sprawdzamy, czy trafienie jest poni¿ej maxSpawnY
            if (leftHit.collider != null && leftHit.point.y <= maxSpawnY)
            {
                Vector2 spawnPos = leftHit.point + Vector2.up * verticalOffset;
                Instantiate(firewavePrefab, spawnPos, Quaternion.identity);
            }

            Vector2 rightCheck = origin + Vector2.right * firewaveSpacing * i;
            RaycastHit2D rightHit = Physics2D.Raycast(rightCheck, Vector2.down, 10f, LayerMask.GetMask("Ground"));

            if (rightHit.collider != null && rightHit.point.y <= maxSpawnY)
            {
                Vector2 spawnPos = rightHit.point + Vector2.up * verticalOffset;
                Instantiate(firewavePrefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(firewaveDelay);
        }
    }

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }
            _walkDirection = value;
        }
    }

    public bool IsAttacking => isAttacking;

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    public bool HasTarget
    {
        get => _HasTarget;
        private set
        {
            _HasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public float AttackCooldown
    {
        get => animator.GetFloat(AnimationStrings.attackCooldown);
        private set => animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    public void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        lastAttackEndTime = Time.time;
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        HasTarget = attackZone.detectedColliders.Count > 0;

        if (HasTarget && AttackCooldown <= 0f)
        {
            animator.SetBool("shouldAttack", true);
            StartAttack();
        }
        else
        {
            animator.SetBool("shouldAttack", false);
        }

        if (damageable.Health <= damageable.Maxhealth / 2)
        {
            fireballSpawnTimer += Time.deltaTime;
            if (fireballSpawnTimer >= fireballSpawnInterval)
            {
                fireballSpawnTimer = 0f;
                SpawnFireballsAbove();
            }
        }
    }


    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            float verticalThreshold = 1.5f;
            float verticalDistance = Mathf.Abs(playerTransform.position.y - transform.position.y);

            if (verticalDistance < verticalThreshold)
            {
                if (playerTransform.position.x < transform.position.x)
                    WalkDirection = WalkableDirection.Left;
                else
                    WalkDirection = WalkableDirection.Right;
            }
        }

        if (HasTarget)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (touchingDirections.isGrounded && touchingDirections.isOnWall)
            {
                FlipDirection();
            }

            if (!damageable.LockVelocity)
            {
                if (CanMove)
                {
                    float targetSpeed = moveSpeed * walkDirectionVector.x;
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + targetSpeed * Time.deltaTime, -maxSpeed, maxSpeed), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, moveStopSpeed), rb.velocity.y);
                }
            }
        }
    }

    private void SpawnFireballsAbove()
    {
        Vector2 center = transform.position;
        float startX = center.x - (fireballSpacing * (fireballCount - 1)) / 2f;

        for (int i = 0; i < fireballCount; i++)
        {
            Vector2 spawnPos = new Vector2(startX + i * fireballSpacing, center.y + fireballHeightOffset);
            Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void FlipDirection()
    {
        if (IsAttacking || Time.time < lastAttackEndTime + flipCooldownAfterAttack)
            return;

        if (HasTarget)
            return;

        WalkDirection = (WalkDirection == WalkableDirection.Right)
            ? WalkableDirection.Left
            : WalkableDirection.Right;
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnDeath()
    {
        StartCoroutine(DelayedDropLoot());
    }

    private IEnumerator DelayedDropLoot()
    {
        LootBag lootBag = GetComponent<LootBag>();
        if (lootBag != null)
        {
            lootBag.DropLoot(transform.position);
        }

        yield return new WaitForSeconds(3f);

        // ZnajdŸ gracza i ustaw jego nextPosition
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                // Ustaw docelow¹ pozycjê gracza w nowej scenie (np. start przy -11.09, -2.96, -0.39)
                pc.nextPosition = new Vector3(-11.09f, -2.96f, -0.39f);
            }
        }

        // Za³aduj now¹ scenê
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }



    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded)
        {
            FlipDirection();
        }
    }
}
