using System.Collections;
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

    [Header("Hit Impact")]
    [SerializeField] private GameObject hitImpact;
    private TrailRenderer trailRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void SetPool(IObjectPool<GameObject> bulletPool)
    {
        pool = bulletPool;
    }

    void OnEnable()
    {
        timer = lifetime;
        isReleased = false;

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
            trailRenderer.Clear();
            StartCoroutine(EnableTrailRoutine());
        }

        Invoke(nameof(SetupVisual), 0.01f);
    }

    private IEnumerator EnableTrailRoutine()
    {
        yield return null; 
        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }
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
                SpawnHitImpact();

                EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                    Vector2 hitDirection = transform.right;
                    enemy.ApplyKnockback(hitDirection);
                }
                ResetAndRelease();
                return; 
            }
        }
        // 2. LOGIKA JIKA INI PELURU MUSUH
        else if (gameObject.CompareTag("EnemyBullet"))
        {
            if (collision.CompareTag("Player"))
            {
                SpawnHitImpact();

                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.PlayerTakeDamage(1);
                }
                ResetAndRelease();
                return; 
            }
        }

        // 3. LOGIKA JIKA MENABRAK TANAH/DINDING (Gunakan 'else if' atau amankan dengan return di atas)
        if (collision.CompareTag("Ground"))
        {
            SpawnHitImpact();
            ResetAndRelease();
        }
    }

    private void SpawnHitImpact()
    {
        if (hitImpact != null)
        {
            GameObject impact = Instantiate(hitImpact, transform.position, Quaternion.identity);
            Destroy(impact, 0.5f);
        }
    }

    private void ResetAndRelease()
    {
        // KEAMANAN GANDA: Jika palang pintu sudah tertutup, abaikan perintah rilis berikutnya
        if (isReleased) return;
        isReleased = true;

        CancelInvoke(); // Batalkan Invoke warna

        if(trailRenderer != null)
        {
            trailRenderer.Clear();
        }

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