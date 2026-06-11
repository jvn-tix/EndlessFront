using UnityEngine;
using UnityEngine.Pool;

public class BulletProjectile : MonoBehaviour
{
    private IObjectPool<GameObject> pool;
    private float lifetime = 1f;
    private float timer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Pengaturan Warna Peluru")]
    public Color playerBulletColor = Color.yellow;
    public Color enemyBulletColor = Color.red;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Perbaikan: langsung ambil dari gameObject
    }

    public void SetPool(IObjectPool<GameObject> bulletPool)
    {
        pool = bulletPool;
    }

    void OnEnable()
    {
        timer = lifetime;
        // Perbaikan: Menghapus tanda kurung berlebih pada nameof
        Invoke(nameof(SetupVisual), 0.01f);
    }

    private void SetupVisual()
    {
        if (spriteRenderer == null) return; // Perbaikan logika: jika null, keluar fungsi agar tidak error

        // Perbaikan: C# sensitif huruf besar, harus CompareTag (C besar)
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
            // Reset dulu sebelum dikembalikan lewat timer waktu habis
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
            }
        }

        // 3. LOGIKA JIKA MENABRAK TANAH/DINDING (Berlaku untuk semua peluru)
        if (collision.CompareTag("Ground"))
        {
            ResetAndRelease();
        }
    }

    private void ResetAndRelease()
    {
        CancelInvoke(); // Batalkan Invoke warna agar tidak berjalan saat peluru sudah di pool

        // Kembalikan Tag peluru ke default "Bullet" sebelum masuk gudang pool
        gameObject.tag = "Bullet";

        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (pool != null)
        {
            pool.Release(gameObject);
        }
    }
}