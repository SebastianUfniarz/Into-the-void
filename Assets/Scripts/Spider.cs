using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;

    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed = 2f;

    private int waypointIndex = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = waypoints[waypointIndex].position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            Vector2 target = waypoints[waypointIndex].position;
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);

            if ((Vector2)transform.position == target)
            {
                waypointIndex++;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Damageable player = collision.collider.GetComponent<Damageable>();
        if (player != null)
        {
            Debug.Log("Spider hit Player via collision!");
            player.Hit(10, Vector2.zero);

            Collider2D spiderCol = GetComponent<Collider2D>();
            Collider2D playerCol = collision.collider;

            Physics2D.IgnoreCollision(spiderCol, playerCol, true);

            StartCoroutine(ReenableCollision(spiderCol, playerCol, 2f));
        }
    }

    private IEnumerator ReenableCollision(Collider2D a, Collider2D b, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(a, b, false);
    }
}
