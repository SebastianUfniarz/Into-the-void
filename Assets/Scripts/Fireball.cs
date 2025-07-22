using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 5f;
    public float speed = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmg = collision.GetComponent<Damageable>();

        if (dmg != null && collision.CompareTag("Player"))
        {
            dmg.Hit(damage, Vector2.zero);
            Destroy(gameObject);
        }
    }
}
