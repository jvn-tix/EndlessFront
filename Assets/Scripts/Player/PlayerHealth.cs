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

    public GameOverMenu gameOverPanel;

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
            gameOverPanel.gameObject.SetActive(false);
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

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(1.5f);
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
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.ShowGameOverUI();
        }

        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
}