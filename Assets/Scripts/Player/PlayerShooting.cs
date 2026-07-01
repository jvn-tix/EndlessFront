using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool; // Wajib ditambahkan untuk menggunakan Object Pool bawaan Unity

public class PlayerShooting : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Atribut Senjata")]
    public float bulletSpeed = 20f;
    public float fireRate = 0.15f;

    private Coroutine shootCoroutine;
    private IObjectPool<GameObject> bulletPool; // Variabel penampung pool
    private Animator animator;

    void Awake()
    {
        // Inisialisasi Object Pool saat game pertama kali dimuat
        bulletPool = new ObjectPool<GameObject>(
            createFunc: CreateBullet,          // Fungsi jika pool kekurangan objek dan harus membuat baru
            actionOnGet: OnTakeBulletFromPool, // Fungsi saat peluru diambil dari pool
            actionOnRelease: OnReturnBulletToPool, // Fungsi saat peluru dikembalikan ke pool
            actionOnDestroy: OnDestroyPoolObject,  // Fungsi jika objek dihancurkan permanen
            collectionCheck: true,
            defaultCapacity: 20,               // Kapasitas awal pool
            maxSize: 50                        // Batas maksimum objek di dalam pool
        );
    }

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        // Kita beri skrip perantara pada peluru agar dia tahu cara kembali ke pool sendiri
        BulletProjectile bulletScript = bullet.AddComponent<BulletProjectile>();
        bulletScript.SetPool(bulletPool);
        return bullet;
    }

    // 2. Fungsi saat peluru diaktifkan kembali
    private void OnTakeBulletFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    // 3. Fungsi saat peluru dinonaktifkan (kembali masuk kotak pool)
    private void OnReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    // 4. Fungsi pembersihan memori jika melebihi maxSize
    private void OnDestroyPoolObject(GameObject bullet)
    {
        Destroy(bullet);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
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

    // Pastikan ada kata 'public' di depannya!
    public GameObject GetBulletFromPool()
    {
        // Sesuaikan 'bulletPool' dengan nama variabel IObjectPool yang kamu buat di skrip ini
        if (bulletPool != null)
        {
            return bulletPool.Get();
        }
        return null;
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

        if(animator != null)
        {   
            Debug.Log("Triggering shoot animation");
            animator.SetTrigger("shoot");
        }

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        float facingDirection = 1f;
        if(playerMovement != null)
        {
            facingDirection = playerMovement.isFacingRight ? 1f : -1f;
        }
        // Ambil peluru dari pool (bukan Instantiate baru)
        GameObject bullet = bulletPool.Get();

        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);

        // Pindahkan posisi dan rotasinya ke laras senjata
        bullet.transform.position = spawnPosition;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }
    }
}