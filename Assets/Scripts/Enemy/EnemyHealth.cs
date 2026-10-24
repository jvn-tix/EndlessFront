using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Atribut Nyawa Musuh")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Visual Efek Hit")]
    public float flashDuration = 0.08f;
    private SpriteRenderer spriteRenderer;
    private Material mat;
    private Coroutine flashCoroutine;
    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.1f;

    [HideInInspector] public bool isKnockback;
    [SerializeField] private GameObject explosionVfx;
    private Rigidbody2D rb;
    private EnemyHealthBar healthBar;

    [SerializeField] private GameObject damagePopup;
    [SerializeField] private HitSquash hitSquash;
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<EnemyHealthBar>();

        if (spriteRenderer != null)
        {
            mat = spriteRenderer.material;
        }
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        isKnockback = false;

        if(mat != null)
        {
            mat.SetFloat(FlashAmountId, 0f);
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

        if (hitSquash != null)
        {
            hitSquash.Play();
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
       mat.SetFloat(FlashAmountId, 1f);
        yield return new WaitForSeconds(flashDuration);
        mat.SetFloat(FlashAmountId, 0f);
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