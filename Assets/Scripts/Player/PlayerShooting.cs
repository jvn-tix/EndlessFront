using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Atribut Senjata")]
    public float bulletSpeed = 20f;
    public float fireRate = 0.15f;

    [Header("MuzzleFlash")]
    [SerializeField] private MuzzleFlash muzzleFlash;

    private Coroutine shootCoroutine;
    private IObjectPool<GameObject> bulletPool;
    private Animator animator;

    void Awake()
    {
        bulletPool = new ObjectPool<GameObject>(
            createFunc: CreateBullet,
            actionOnGet: OnTakeBulletFromPool,
            actionOnRelease: OnReturnBulletToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 50
        );
    }

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        BulletProjectile bulletScript = bullet.GetComponent<BulletProjectile>();
        if (bulletScript == null)
        {
            bulletScript = bullet.AddComponent<BulletProjectile>();
        }

        bulletScript.SetPool(bulletPool);
        return bullet;
    }

    private void OnTakeBulletFromPool(GameObject bullet)
    {
        if (bullet == null) return;
        bullet.SetActive(true);
    }

    private void OnReturnBulletToPool(GameObject bullet)
    {
        if (bullet == null) return;
        bullet.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject bullet)
    {
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }

    public GameObject GetBulletFromPool()
    {
        if (bulletPool != null)
        {
            return bulletPool.Get();
        }
        return null;
    }   

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (GameplayMenuUI.isPaused) return;

        if (context.started)
        {
            if (shootCoroutine == null) shootCoroutine = StartCoroutine(FireRoutine());
        }
        else if (context.canceled)
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
        }
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null) return;

        if (animator != null)
        {
            animator.SetTrigger("shoot");
        }

        GameObject bullet = bulletPool.Get();
        if (bullet == null) return;

        // 1. Ambil arah hadap langsung dari PlayerMovement
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();

        float directionX = (playerMovement != null && !playerMovement.isFacingRight) ? -1f : 1f;

        // 2. Ambil posisi spawn dari firePoint
        Vector3 spawnPosition = (firePoint != null)
            ? firePoint.position
            : new Vector3(transform.position.x + (directionX * 0.5f), transform.position.y + 0.2f, transform.position.z);

        bullet.transform.SetParent(null);
        bullet.transform.position = spawnPosition;

        // Putar visual peluru jika menghadap kiri
        float angle = (directionX < 0) ? 180f : 0f;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(directionX * bulletSpeed, 0f);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Flash();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.shootSfx);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.2f);
        }
    }
}