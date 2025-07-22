using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public Vector2 direction = Vector2.right;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable target = collision.GetComponent<Damageable>();
        if (target != null && collision.CompareTag("Player"))
        {
            target.Hit(damage, Vector2.zero);
            Destroy(gameObject);
        }
    }
}
