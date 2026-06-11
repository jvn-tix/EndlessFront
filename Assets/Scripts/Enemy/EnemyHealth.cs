using System.Collections;
using UnityEngine;
using UnityEngine.Pool; // Wajib ditambahkan

public class EnemyHealth : MonoBehaviour
{
    [Header("Atribut Nyawa Musuh")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Visual Efek Hit")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor = Color.lightBlue;

    // Referensi ke pool yang melahirkannya
    private IObjectPool<GameObject> myPool;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Fungsi untuk mencatat pool (dipanggil oleh Spawner saat spawn)
    public void SetPool(IObjectPool<GameObject> pool)
    {
        myPool = pool;
    }

    // Fungsi ini dipanggil otomatis setiap kali musuh diaktifkan dari pool
    void OnEnable()
    {
        currentHealth = maxHealth;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor; // Reset warna ke asli
        }
    }

    void Start()
    {
        if (spriteRenderer != null && originalColor == Color.clear)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (spriteRenderer != null)
        {
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

    private void Die()
    {
        if (myPool != null)
        {
            // Kembalikan ke pool (Nonaktifkan), JANGAN di-Destroy
            myPool.Release(gameObject);
        }
        else
        {
            // Jaga-jaga kalau dipasang manual tanpa spawner
            Destroy(gameObject);
        }
    }
}