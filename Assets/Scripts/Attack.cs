using UnityEngine;

public class Attack : MonoBehaviour
{
    public Vector2 knockback = Vector2.zero;
    private ICharacterStats attackerStats;

    private void Awake()
    {
        attackerStats = GetComponent<ICharacterStats>() ?? GetComponentInParent<ICharacterStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<Damageable>();
        if (damageable == null) return;

        int damage = attackerStats?.Attack ?? 10;
        Vector2 direction = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

        if (damageable.Hit(damage, direction))
        {
            Debug.Log($"{collision.name} hit for {damage} damage");
        }
    }
}