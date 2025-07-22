using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class IceBeast : MonoBehaviour, IDeathHandler
{
    public float moveSpeed = 4f;
    public float maxSpeed = 4f;
    public float moveStopSpeed = 0.6f;
    public float knockbackForce = 10f;
    public float verticalThreshold = 1.5f;

    private Rigidbody2D rb;
    private TouchingDirections touchingDirections;
    private Animator animator;
    private Damageable damageable;

    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public DetectionZone aboveZone; // <-- Nowa strefa wykrywania nad głową

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    public bool _HasTarget = false;
    private bool isAttacking = false;
    private float lastAttackEndTime = -Mathf.Infinity;
    public float flipCooldownAfterAttack = 0.3f;

    [Header("IceWave Spawn Settings")]
    public GameObject iceWavePrefab;
    public int iceWaveCountPerSide = 5;
    public float iceWaveSpacing = 1f;
    public float iceWaveDelay = 0.1f;

    public float attackCooldown = 10f;
    private float cooldownTimer = 0f;

    private Transform playerTransform;

    private bool isPlayerAbove = false;
    public bool IsPlayerAbove
    {
        get => isPlayerAbove;
        private set
        {
            isPlayerAbove = value;
            animator.SetBool("isPlayerAbove", value); // do animacji (opcjonalnie)
        }
    }

    public WalkableDirection WalkDirection
    {
        get => _walkDirection;
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

    private void Update()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }

        HasTarget = attackZone != null && attackZone.detectedColliders.Count > 0;

        // Użycie strefy nad głową
        IsPlayerAbove = aboveZone != null && aboveZone.detectedColliders.Count > 0;

        if (HasTarget && !IsPlayerAbove)
        {
            animator.SetBool("shouldAttack", true);
            StartAttack();
        }
        else
        {
            animator.SetBool("shouldAttack", false);
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayerAbove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (playerTransform != null)
        {
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
                    float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
                    rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, moveStopSpeed), rb.velocity.y);
                }
            }
        }
    }

    public void StartAttack()
    {
        if (isAttacking) return;
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        StartCoroutine(IcewaveAttackRoutine());
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        lastAttackEndTime = Time.time;
    }

    private IEnumerator IcewaveAttackRoutine()
    {
        yield return IcewaveSpreadCoroutine();
        EndAttack();
        AttackCooldown = attackCooldown;
    }

    private IEnumerator IcewaveSpreadCoroutine()
    {
        Vector2 feetPos = new Vector2(transform.position.x + 1f, GetComponent<Collider2D>().bounds.min.y);
        float verticalOffset = 1f;

        for (int i = 0; i < iceWaveCountPerSide; i++)
        {
            Vector2 leftCheck = feetPos + Vector2.left * iceWaveSpacing * i;
            RaycastHit2D leftHit = Physics2D.Raycast(leftCheck, Vector2.down, 10f, LayerMask.GetMask("Ground"));
            if (leftHit.collider != null)
            {
                Vector2 spawnPos = leftHit.point + Vector2.up * verticalOffset;
                Instantiate(iceWavePrefab, spawnPos, Quaternion.identity);
            }

            Vector2 rightCheck = feetPos + Vector2.right * iceWaveSpacing * i;
            RaycastHit2D rightHit = Physics2D.Raycast(rightCheck, Vector2.down, 10f, LayerMask.GetMask("Ground"));
            if (rightHit.collider != null)
            {
                Vector2 spawnPos = rightHit.point + Vector2.up * verticalOffset;
                Instantiate(iceWavePrefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(iceWaveDelay);
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

    public void OnDeath()
    {
        StartCoroutine(DelayedDropLoot());
    }

    private IEnumerator DelayedDropLoot()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        yield return new WaitForSeconds(2f);

        LootBag lootBag = GetComponent<LootBag>();
        if (lootBag != null)
        {
            lootBag.DropLoot(transform.position + Vector3.up * 0.5f);
        }

        yield return new WaitForSeconds(50000f);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.nextPosition = new Vector3(-11.09f, -2.96f, -0.39f);
            }
        }

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
