using UnityEngine;
using UnityEngine.Pool;

public class BulletProjectile : MonoBehaviour
{
    private IObjectPool<GameObject> pool;
    private float lifetime = 1f;
    private float timer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // PENGAMAN UTAMA: Palang pintu agar tidak rilis 2 kali
    private bool isReleased;

    [Header("Pengaturan Warna Peluru")]
    public Color playerBulletColor = Color.yellow;
    public Color enemyBulletColor = Color.red;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPool(IObjectPool<GameObject> bulletPool)
    {
        pool = bulletPool;
    }

    void OnEnable()
    {
        timer = lifetime;
        isReleased = false; // Reset status setiap kali peluru keluar dari pool
        Invoke(nameof(SetupVisual), 0.01f);
    }

    private void SetupVisual()
    {
        if (spriteRenderer == null) return;

        if (gameObject.CompareTag("EnemyBullet"))
        {
            spriteRenderer.color = enemyBulletColor;
        }
        else
        {
            spriteRenderer.color = playerBulletColor;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ResetAndRelease();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. LOGIKA JIKA INI PELURU PLAYER
        if (gameObject.CompareTag("Bullet"))
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
                ResetAndRelease();
                return; // Langsung keluar fungsi jika sudah rilis
            }
        }
        // 2. LOGIKA JIKA INI PELURU MUSUH
        else if (gameObject.CompareTag("EnemyBullet"))
        {
            if (collision.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.PlayerTakeDamage(1);
                }
                ResetAndRelease();
                return; // Langsung keluar fungsi jika sudah rilis
            }
        }

        // 3. LOGIKA JIKA MENABRAK TANAH/DINDING (Gunakan 'else if' atau amankan dengan return di atas)
        if (collision.CompareTag("Ground"))
        {
            ResetAndRelease();
        }
    }

    private void ResetAndRelease()
    {
        // KEAMANAN GANDA: Jika palang pintu sudah tertutup, abaikan perintah rilis berikutnya
        if (isReleased) return;
        isReleased = true;

        CancelInvoke(); // Batalkan Invoke warna

        // Kembalikan Tag peluru ke default sebelum masuk pool
        gameObject.tag = "Bullet";

        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (pool != null)
        {
            pool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}