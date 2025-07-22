using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Archer : MonoBehaviour
{
    public float moveSpeed = 4f;

    private Rigidbody2D rb;
    private TouchingDirections touchingDirections;
    private Animator animator;
    private Damageable damageable;

    public DetectionZone attackZone;
    public CliffDetectionZone cliffDetectionZone;

    public GameObject projectilePrefab;
    public Transform firePoint;

    public float attackCooldown = 2f;
    private float currentCooldown = 0f;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 walkDirectionVector = Vector2.right;

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

    public bool HasTarget => attackZone.detectedColliders.Count > 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        WalkDirection = WalkableDirection.Right;
    }

    private void Update()
    {
        animator.SetBool("hasTarget", HasTarget);

        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }
        if (HasTarget)
        {
            FaceTarget(); // <--- Dodaj to
        }
    }

    private void FixedUpdate()
    {
        if (HasTarget)
        {
            rb.velocity = Vector2.zero;
            if (currentCooldown <= 0f)
            {
                animator.SetTrigger("attack");
                currentCooldown = attackCooldown;
            }

            return;
        }

        if (touchingDirections.isGrounded && touchingDirections.isOnWall)
        {
            FlipDirection();
        }

        Vector2 targetPos = rb.position + walkDirectionVector * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    private void FlipDirection()
    {
        if (HasTarget) return;

        WalkDirection = (WalkDirection == WalkableDirection.Right)
            ? WalkableDirection.Left
            : WalkableDirection.Right;
    }

    public void FireProjectile()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();

        p.direction = (WalkDirection == WalkableDirection.Right) ? Vector2.right : Vector2.left;

        if (WalkDirection == WalkableDirection.Left)
        {
            proj.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FaceTarget()
    {
        if (attackZone.detectedColliders.Count == 0) return;

        Transform target = attackZone.detectedColliders[0].transform;

        if (target != null)
        {
            if (target.position.x > transform.position.x)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else
            {
                WalkDirection = WalkableDirection.Left;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Damageable player = collision.collider.GetComponent<Damageable>();
        if (player != null)
        {
            Debug.Log("Archer hit Player via collision!");

            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;

            player.Hit(10, knockbackDir);

            Collider2D archerCol = GetComponent<Collider2D>();
            Collider2D playerCol = collision.collider;

            Physics2D.IgnoreCollision(archerCol, playerCol, true);
            StartCoroutine(ReenableCollision(archerCol, playerCol, 2f));
        }
    }

    private IEnumerator ReenableCollision(Collider2D a, Collider2D b, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(a, b, false);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded)
        {
            FlipDirection();
        }
    }
}
