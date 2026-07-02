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
    private Color originalColor = Color.white; // Ganti default ke White agar warna asli sprite tidak berubah di awal

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // 1. Fungsi OnEnable disederhanakan (Murni untuk reset nyawa saat objek lahir)
    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Catat warna asli dari Sprite asli di Inspector
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (spriteRenderer != null)
        {
            // Stop coroutine yang sedang berjalan agar efek flash tidak tumpang tindih kalau ditembak cepat
            StopAllCoroutines();
            StartCoroutine(FlashRedRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
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
            ScoreManager.Instance.AddScore(1); // Memberikan 100 poin tiap musuh mati
        }

        Destroy(gameObject);
    }
}