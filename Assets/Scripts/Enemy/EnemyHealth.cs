using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Atribut Nyawa Musuh")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Visual Efek Hit")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor = Color.white;
    private Coroutine flashCoroutine;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.1f;

    [HideInInspector] public bool isKnockback;
    [SerializeField] private GameObject explosionVfx;
    private Rigidbody2D rb;
    private EnemyHealthBar healthBar;

    [SerializeField] private GameObject damagePopup;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<EnemyHealthBar>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        isKnockback = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // 1. Spawn Damage Popup
        if (damagePopup != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.5f, 0);
            GameObject popup = Instantiate(damagePopup, spawnPos, Quaternion.identity);
            DamagePopup popupCom = popup.GetComponent<DamagePopup>();

            if (popupCom != null)
            {
                popupCom.Setup(damage);
            }
        }

        // 2. Update UI Health Bar
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // 3. Efek Kedip Merah (Hanya menghentikan flash sebelumnya, bukan knockback)
        if (spriteRenderer != null)
        {
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(FlashRedRoutine());
        }

        // 4. Cek Kematian
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector2 hitDirection)
    {
        if (rb == null || isKnockback) return;
        StartCoroutine(KnockbackRoutine(hitDirection));
    }

    private IEnumerator KnockbackRoutine(Vector2 hitDirection)
    {
        isKnockback = true;

        // Beri dorongan searah laju peluru
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockback = false;
    }

    private IEnumerator FlashRedRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }

    private void Die()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }

        if (HitStop.Instance != null)
        {
            HitStop.Instance.Freeze(0.04f);
        }

        if (explosionVfx != null)
        {
            Instantiate(explosionVfx, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}