using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.1f;

    [HideInInspector]public bool isKnockback;
    [SerializeField] private GameObject explosionVfx;
    private Rigidbody2D rb;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        isKnockback = false;
    }

    void Start()
    {
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (spriteRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashRedRoutine());
        }

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
    }

    // 2. Perbaikan Total pada Fungsi Die()
    private void Die()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1); 
        }

        if(HitStop.Instance != null)
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