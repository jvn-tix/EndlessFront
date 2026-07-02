using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Pengaturan Darah")]
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;

    [Header("Visual Efek Hit")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public GameObject gameOverPanel;

    void Awake()
    {
        // Ambil SpriteRenderer dari objek Player
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.setMaxHearts(maxHealth);
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player terkena damage! Sisa darah: {currentHealth}");
        healthUI.updateHearts(currentHealth);

        // Jalankan efek berkedip merah
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRedRoutine());
        }

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private IEnumerator FlashRedRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void PlayerDie()
    {
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
}