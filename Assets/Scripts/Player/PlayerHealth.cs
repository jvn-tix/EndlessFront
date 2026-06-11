using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Pengaturan Darah")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Visual Efek Hit")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        // Ambil SpriteRenderer dari objek Player
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player terkena damage! Sisa darah: {currentHealth}");

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
        Debug.Log("Player telah mati! Game Over.");
        gameObject.SetActive(false);
    }
}