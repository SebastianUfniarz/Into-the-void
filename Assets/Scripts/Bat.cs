using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bat : MonoBehaviour
{
    public float speed = 3f;
    public DetectionZone detectionZone;
    public int damage = 10;
    public float knockbackForce = 5f;
    public float collisionCooldown = 1f;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;
    private bool isPlayerInRange => detectionZone.detectedColliders.Count > 0;
    public bool HasTarget => detectionZone.detectedColliders.Count > 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isPlayerInRange)
        {
            animator.SetBool("hasTarget", false);
            player = null;
            return;
        }

        if (player == null && detectionZone.detectedColliders.Count > 0)
        {
            animator.SetBool("hasTarget", true);
            player = detectionZone.detectedColliders[0].transform;
        }

        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            Flip();
        }
    }

    private void Flip()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        if (transform.position.x > player.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Damageable playerDamageable = collision.collider.GetComponent<Damageable>();
        if (playerDamageable != null)
        {
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized * knockbackForce;

            playerDamageable.Hit(damage, knockbackDir);
            Debug.Log("Bat hit the player!");

            Collider2D batCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = collision.collider;
            Physics2D.IgnoreCollision(batCollider, playerCollider, true);
            StartCoroutine(ReenableCollision(batCollider, playerCollider, collisionCooldown));
        }
    }

    private IEnumerator ReenableCollision(Collider2D a, Collider2D b, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(a, b, false);
    }
}
