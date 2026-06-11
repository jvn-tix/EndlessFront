using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Daftar Variasi Musuh")]
    [Tooltip("Masukkan Prefab Melee dan Ranged kamu di sini")]
    public GameObject[] enemyPrefabs;

    [Header("Pengaturan Waktu Spawn")]
    public float spawnInterval = 3f; // Jeda waktu antar spawn musuh (detik)

    [Header("Titik Spawn Acak (Kiri & Kanan)")]
    public Transform[] spawnPoints;

    private float nextSpawnTime;
    private Transform playerTransform;

    // Kamus pool untuk mengurus pooling masing-masing prefab musuh
    private Dictionary<GameObject, IObjectPool<GameObject>> enemyPools = new Dictionary<GameObject, IObjectPool<GameObject>>();

    void Start()
    {
        // Cari player di awal game menggunakan Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Buat pool otomatis untuk setiap prefab musuh yang didaftarkan
        if (enemyPrefabs != null && enemyPrefabs.Length > 0)
        {
            foreach (var prefab in enemyPrefabs)
            {
                if (prefab != null && !enemyPools.ContainsKey(prefab))
                {
                    CreatePoolForPrefab(prefab);
                }
            }
        }

        // Tentukan jadwal spawn pertama
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void CreatePoolForPrefab(GameObject prefab)
    {
        IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: (obj) => {
                // LOGIKA RANDOM POSISI: Memilih lokasi secara acak dari list spawnPoints
                if (spawnPoints != null && spawnPoints.Length > 0)
                {
                    int randomIndex = Random.Range(0, spawnPoints.Length);
                    Vector3 spawnPos = spawnPoints[randomIndex].position;
                    obj.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0f);
                }
                else
                {
                    obj.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                }

                obj.SetActive(true);
            },
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 30
        );

        enemyPools.Add(prefab, pool);
    }

    void Update()
    {
        // KONDISI BERHENTI: Jika player sudah mati (null atau tidak aktif), spawner berhenti total
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy) return;

        // Munculkan musuh secara berkala tanpa henti
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomEnemyFromPool();
            nextSpawnTime = Time.time + spawnInterval; // Reset timer spawn berikutnya
        }
    }

    private void SpawnRandomEnemyFromPool()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // LOGIKA RANDOM MUSUH: Pilih secara acak prefab mana yang mau dikeluarkan (Melee/Ranged)
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomEnemyIndex];

        if (selectedPrefab == null) return;

        // Ambil dari laci pool prefab yang terpilih
        if (enemyPools.TryGetValue(selectedPrefab, out IObjectPool<GameObject> pool))
        {
            GameObject enemy = pool.Get();
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.SetPool(pool);
            }
        }
    }
}