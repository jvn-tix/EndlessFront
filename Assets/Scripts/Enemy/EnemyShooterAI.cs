using UnityEngine;

public class EnemyShooterAI : MonoBehaviour
{
    [Header("Referensi")]
    public Transform firePoint;

    [Header("Atribut Tembakan")]
    public float fireRate = 1.5f;
    public float bulletSpeed = 8f;
    public float shootingRange = 6f;

    private float fireTimer;
    private Transform playerTransform;
    private PlayerShooting playerShootingScript;
    private Animator animator;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerShootingScript = player.GetComponentInChildren<PlayerShooting>();
        }
        fireTimer = fireRate;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Jika player masuk dalam jangkauan tembak, jalankan timer
        if (distanceToPlayer <= shootingRange)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                EnemyShoot();
                fireTimer = fireRate; // Reset jeda tembak
            }
        }
    }

    private void EnemyShoot()
    {
        if (firePoint == null || playerShootingScript == null) return;

        GameObject bullet = playerShootingScript.GetBulletFromPool();

        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.identity;

            bullet.tag = "EnemyBullet";

            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            if (rbBullet != null)
            {
                float directionSign = Mathf.Sign(transform.localScale.x);
                rbBullet.linearVelocity = new Vector2(directionSign * bulletSpeed, 0f);

                // SINKRONISASI TRIGGER: Panggil parameter "isShooting" saat peluru keluar
                if (animator != null)
                {
                    animator.SetTrigger("shoot");
                }
            }
        }
    }
}