using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class DemonEnemy : MonoBehaviour
{
    public float moveSpeed = 4f;                            // Pr�dko�� poruszania rycerza
    public float maxSpeed = 4f;                             // Maksymalna pr�dko�� rycerza
    public float moveStopSpeed = 0.6f;                      // Pr�dko�� zatrzymywania
    public float knockbackForce = 10f;                      // Si�a knockbacku

    Rigidbody2D rb;                                         // Odno�nik do Rigidbody2D rycerza
    TouchingDirections touchingDirections;                  // Odno�nik do komponentu wykrywaj�cego dotkni�cia
    Animator animator;                                      // Odno�nik do animatora
    Damageable damageable;                                  // Odno�nik do komponentu Damageable

    public DetectionZone attackZone;                        // Strefa ataku
    public DetectionZone cliffDetectionZone;                // Strefa wykrywania klifu
    public enum WalkableDirection { Right, Left }           // Kierunki poruszania si�
    private WalkableDirection _walkDirection;               // Kierunek poruszania si�
    private Vector2 walkDirectionVector = Vector2.left;    // Wektor kierunku poruszania si�
    public bool _HasTarget = false;                         // Flaga informuj�ca o posiadaniu celu
    
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
    public float firewaveDelay = 0.1f; // op�nienie mi�dzy spawnami

    public void SpawnFirewaveLine()
    {
        StartCoroutine(FirewaveSpreadCoroutine());
    }

    private IEnumerator FirewaveSpreadCoroutine()
    {
        Vector2 origin = transform.position + new Vector3(5f, 0f, 0f);
        float verticalOffset = 1f;

        for (int i = 0; i < firewaveCountPerSide; i++)
        {
            // Lewa pozycja
            Vector2 leftCheck = origin + Vector2.left * firewaveSpacing * i;
            RaycastHit2D leftHit = Physics2D.Raycast(leftCheck, Vector2.down, 10f, LayerMask.GetMask("Ground"));
            if (leftHit.collider != null)
            {
                Vector2 spawnPos = leftHit.point + Vector2.up * verticalOffset;
                Instantiate(firewavePrefab, spawnPos, Quaternion.identity);
            }

            // Prawa pozycja z lekkim przesuni�ciem w prawo
            Vector2 rightCheck = origin + Vector2.right * firewaveSpacing * i;
            RaycastHit2D rightHit = Physics2D.Raycast(rightCheck, Vector2.down, 10f, LayerMask.GetMask("Ground"));
            if (rightHit.collider != null)
            {
                Vector2 spawnPos = rightHit.point + Vector2.up * verticalOffset;
                Instantiate(firewavePrefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(firewaveDelay);
        }
    }



    // W�a�ciwo�� ustawiaj�ca kierunek poruszania si� i obracaj�ca posta�
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                // Obracanie postaci
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public bool HasTarget
    {
        get { return _HasTarget; }
        private set
        {
            _HasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        private set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
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
        // Sprawdzenie, czy rycerz ma cel w zasi�gu
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        
        fireballSpawnTimer += Time.deltaTime;

        if (fireballSpawnTimer >= fireballSpawnInterval)
        {
            fireballSpawnTimer = 0f;
            SpawnFireballsAbove();
        }
    }

    private void FixedUpdate()
    {
        if (HasTarget)
        {
            // Zatrzymywanie rycerza, gdy ma cel
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Normalne poruszanie, je�li rycerz nie ma celu
            if (touchingDirections.isGrounded && touchingDirections.isOnWall)
            {
                FlipDirection();
            }

            // Poruszanie, je�li nie ma zablokowanej pr�dko�ci i mo�na si� porusza�
            if (!damageable.LockVelocity)
            {
                if (CanMove)
                {
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (moveSpeed * walkDirectionVector.x * Time.deltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
                }
                else
                {
                    // Stopniowe zatrzymywanie, je�li nie mo�na si� porusza�
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

        // Tu mo�esz odpali� jak�� animacj�, efekt lub d�wi�k
    }

    private void FlipDirection()
    {
        // Nie obracaj postaci, je�li rycerz atakuje
        if (HasTarget) return;

        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.Log("Current walkable direction is not set to legal values of right and left");
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded)
        {
            FlipDirection();
        }
    }
}
