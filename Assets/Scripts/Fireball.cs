using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Samo-destrukcja po czasie
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sprawdzamy, czy obiekt ma Damageable
        Damageable dmg = collision.GetComponent<Damageable>();

        if (dmg != null && collision.CompareTag("Player"))
        {
            dmg.Hit(damage, Vector2.zero); // Mo¿esz tu dodaæ knockback jeœli chcesz
            Destroy(gameObject); // Fireball znika po trafieniu
        }
    }
}
