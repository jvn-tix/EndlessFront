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

    public GameplayMenuUI gameplayMenu;

    void Awake()
    {
        // Ambil SpriteRenderer dari objek Player
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;

        if(healthUI != null)
        {
            healthUI.setMaxHearts(maxHealth);
        }
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
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

        if(HitStop.Instance != null)
        {
            HitStop.Instance.Freeze(0.06f);
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
        if(gameplayMenu != null)
        {
            gameplayMenu.ShowGameOverUI();
        }

        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
}