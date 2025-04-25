using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Firewave : MonoBehaviour
{
    public int damage = 20;
    private Rigidbody2D rb;
    private TouchingDirections touching;

    private bool isLanded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touching = GetComponent<TouchingDirections>();
    }

    private void Update()
    {
        if (!isLanded && touching.isGrounded)
        {
            Land();
        }
    }

    private void Land()
    {
        isLanded = true;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // Zatrzymuje fizykê – nie spadnie, nie przesunie siê
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmg = collision.GetComponent<Damageable>();

        if (dmg != null && collision.CompareTag("Player"))
        {
            dmg.Hit(damage, Vector2.zero);
            // Nie niszczymy, zostaje na ziemi
        }
    }
}
