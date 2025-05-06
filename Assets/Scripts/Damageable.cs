using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damagableHit;
    public UnityEvent<int, int> healthChanged;
    public UnityEvent<int, int> manaChanged;
    public UnityEvent<GameObject, int, string> damageOverTimeApplied;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _health = 100;
    [SerializeField] private int _maxMana = 100;
    [SerializeField] private int _mana = 100;
    private PlayerStats playerStats;
    private float alpha = 1f;
    private bool _isAlive = true;
    public bool isInvincible = false;
    private float timeSinceHit = 0;
    public float invincibilityTime = 3f;
    public int baseCriticalChance = 4;
    public float criticalMultiplier = 2f;

    private Animator animator;
    private Rigidbody2D rb;
    private Renderer rend;
    private ICharacterStats characterStats;
    private Color originalColor;

    public int Maxhealth
    {
        get => _maxHealth;
        internal set
        {
            _maxHealth = value;
            healthChanged?.Invoke(Health, Maxhealth);
        }
    }

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, Maxhealth);
            healthChanged?.Invoke(Health, Maxhealth);
            if (_health <= 0) IsAlive = false;
        }
    }

    public int MaxMana
    {
        get => _maxMana;
        set
        {
            _maxMana = value;
            manaChanged?.Invoke(Mana, MaxMana);
        }
    }

    public int Mana
    {
        get => _mana;
        set
        {
            _mana = Mathf.Clamp(value, 0, MaxMana);
            manaChanged?.Invoke(Mana, MaxMana);
        }
    }

    public bool LockVelocity
    {
        get => animator.GetBool(AnimationStrings.lockVelocity);
        set => animator.SetBool(AnimationStrings.lockVelocity, value);
    }

    public bool IsAlive
    {
        get => _isAlive;
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set to " + value);

            if (!value)
            {
                rb.gravityScale = 500;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                var playerInput = GetComponent<PlayerInput>();
                if (playerInput != null) playerInput.enabled = false;
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        characterStats = GetComponent<ICharacterStats>();
        originalColor = rend.material.color;
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        if (isInvincible && IsAlive)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit >= invincibilityTime) DisableInvincibility();
            else HandleInvincibilityEffect();
        }
    }

    public IEnumerator DealDamageOverTime(Damageable target, int damagePerTick, float interval, float duration, string damageType)
    {
        float elapsed = 0f;
        while (elapsed < duration && target != null && target.IsAlive)
        {
            target.Hit(damagePerTick, Vector2.zero);
            damageOverTimeApplied?.Invoke(target.gameObject, damagePerTick, damageType);
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
    }

    public void UseMana(int amount)
    {
        if (Mana >= amount)
        {
            Mana -= amount;
            Debug.Log($"Used {amount} mana. Remaining: {Mana}/{MaxMana}");
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (!IsAlive || isInvincible) return false;

        bool isCritical = Random.Range(0f, 100f) < baseCriticalChance;
        int finalDamage = isCritical ? Mathf.RoundToInt(damage * criticalMultiplier) : damage;

        int defense = 0;
        if (characterStats != null)
            defense = characterStats.Defense;
        else if (playerStats != null)
            defense = playerStats.Defense;

        int damageAfterDefense = Mathf.Max(damage - defense, 1);
        Health -= damageAfterDefense;

        Debug.Log($"Damage: {finalDamage} -> After defense ({defense}): {damageAfterDefense}");


        animator.SetTrigger(AnimationStrings.hitTrigger);
        damagableHit?.Invoke(damageAfterDefense, knockback);
        CharacterEvents.characterDamaged.Invoke(gameObject, damageAfterDefense);

        return true;
    }

    private void EnableInvincibility()
    {
        isInvincible = true;
        timeSinceHit = 0;
        Physics2D.IgnoreLayerCollision(7, 8, true);
        alpha = 1f;
    }

    private void HandleInvincibilityEffect()
    {
        float fadeSpeed = 3f;
        alpha = Mathf.PingPong(Time.time * fadeSpeed, 0.7f) + 0.3f;
        rend.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }

    private void DisableInvincibility()
    {
        isInvincible = false;
        timeSinceHit = 0;
        Physics2D.IgnoreLayerCollision(7, 8, false);
        rend.material.color = originalColor;
    }

    public bool Heal(int healthRestore)
    {
        if (!IsAlive || Health >= Maxhealth) return false;

        int actualHeal = Mathf.Min(healthRestore, Maxhealth - Health);
        Health += actualHeal;
        CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
        return true;
    }

    public void IncreaseMaxHealth(int additionalHealth)
    {
        Maxhealth += additionalHealth;
        Health = Mathf.Min(Health, Maxhealth);
    }
}