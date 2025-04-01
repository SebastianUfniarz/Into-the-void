using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Damageable : MonoBehaviour
{
 
    public UnityEvent<int, Vector2> damagableHit;  
    public UnityEvent<int, int> healthChanged;
    public UnityEvent<GameObject, int, string> damageOverTimeApplied; 

    [SerializeField] private int _maxHealth = 100;  // Maksymalne zdrowie
    [SerializeField] private int _health = 100;     // Aktualne zdrowie
    [SerializeField] private int _maxMana = 100;    // Maksymalne zdrowie
    [SerializeField] private int _mana = 100;       // Aktualne zdrowie
    private float alpha = 1f;                       // Poziom przezroczysto�ci podczas nietykalno�ci
    private bool _isAlive = true;                   // Czy obiekt �yje
    public bool isInvincible = false;               // Czy obiekt jest nietykalny
    private float timeSinceHit = 0;                 // Czas od ostatniego trafienia
    public float invincibilityTime = 3f;            // Czas trwania nietykalno�ci po otrzymaniu obra�e�
    public int baseCriticalChance = 4;              // 4% bazowej szansy na krytyka (jak w Terrarii)
    public float criticalMultiplier = 2f;           // Mno�nik obra�e� krytycznych (x2)
    private Animator animator;
    private Rigidbody2D rb;
    private Renderer rend;
    private PlayerStats playerstats;
    private Color originalColor;



    public int Maxhealth
    {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }

    public int Health
    {
        get { return _health; }
        private set
        {
            _health = value;
            healthChanged?.Invoke(_health, Maxhealth);
            if (_health <= 0) isAlive = false;
        }
    }
    public int Maxmana
    {
        get { return _maxMana; }
        private set { _maxMana = value; }
    }

    public int Mana
    {
        get { return _mana; }
        private set
        {
            _mana = value;
            healthChanged?.Invoke(_mana, Maxmana);
        }
    }

    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
    }

    public bool isAlive
    {
        get { return _isAlive; }
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set to " + value);

            if (!value)
            {
                // Po �mierci blokuje ruch i wy��cza kontrol� gracza
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
        playerstats = FindObjectOfType<PlayerStats>();
        originalColor = rend.material.color;
    }

    private void Update()
    {       
        if (isInvincible && isAlive)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit >= invincibilityTime) DisableInvincibility();
            else HandleInvincibilityEffect();
        }
    }
    public IEnumerator DealDamageOverTime(Damageable target, int damagePerTick, float interval, float duration, string damageType)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (target != null && target.isAlive)
            {
                target.Hit(damagePerTick, Vector2.zero);

                // Wywo�anie eventu dla obra�e� w czasie
                damageOverTimeApplied?.Invoke(target.gameObject, damagePerTick, damageType); // Wywo�ujemy event obra�e� w czasie
            }

            yield return new WaitForSeconds(interval); // Poczekaj na nast�pny "tick"
            elapsed += interval;
        }
    }

    public void UseMana(int amount)
    {
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null && damageable.Mana >= amount)
        {
            damageable.Mana -= amount;
            Debug.Log($"U�yto {amount} many. Pozosta�o: {damageable.Mana}/{damageable.Maxmana}");
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (isAlive && !isInvincible)
        {
            bool isCritical = Random.Range(0f, 100f) < baseCriticalChance;
            int finalDamage = damage;

            if (isCritical)
            {
                finalDamage = Mathf.RoundToInt(damage * criticalMultiplier);
                Debug.Log($"KRYTYK! Zadano {finalDamage} obra�e� (bazowo: {damage})");
            }

            int damageAfterDefense = Mathf.Max(finalDamage - playerstats.defense, 1);
            Debug.Log($"Obra�enia: {finalDamage} -> Po redukcji (defense {playerstats.defense}): {damageAfterDefense}");

            Health -= damageAfterDefense;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            damagableHit?.Invoke(damageAfterDefense, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damageAfterDefense);

            return true;
        }
        return false;
    }

    // W��czenie nietykalno�ci, ignorowanie kolizji mi�dzy okre�lonymi warstwami czyli 7 to gracz 8 to przeciwnik
    private void EnableInvincibility()
    {
        isInvincible = true;
        timeSinceHit = 0;
        Physics2D.IgnoreLayerCollision(7, 8, true);
        alpha = 1f;
    }

   
    private void HandleInvincibilityEffect()
    {
        float fadeSpeed = 3f;  // Szybko�� migotania przezroczysto�ci
        alpha = Mathf.PingPong(Time.time * fadeSpeed, 0.7f) + 0.3f;  // Zakres migotania od 0.3 do 1
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
        if (isAlive && Health < Maxhealth)
        {
            int maxHeal = Mathf.Max(Maxhealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal); 
            return true;
        }
        return false;
    }

    public void IncreaseMaxHealth(int additionalHealth)
    {
        Maxhealth += additionalHealth;
        Health = Mathf.Min(Health, Maxhealth);

        // Wywo�ujemy healthChanged, aby zaktualizowa� maksymalne zdrowie w UI
        healthChanged.Invoke(Health, Maxhealth);
    }
}
