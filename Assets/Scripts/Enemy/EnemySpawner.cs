using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab Musuh")]
    [Tooltip("Tarik 1 prefab musuhmu ke sini")]
    public GameObject enemyPrefab;

    [Header("Pengaturan Waktu Spawn")]
    public float spawnInterval = 3f; // Jeda waktu antar spawn musuh (detik)

    [Header("Titik Spawn Acak (Kiri & Kanan)")]
    public Transform[] spawnPoints;

    private float nextSpawnTime;
    private Transform playerTransform;

    void Start()
    {
        // Cari player di awal game menggunakan Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Tentukan jadwal spawn pertama
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // KONDISI BERHENTI: Jika player sudah mati, spawner berhenti total
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy) return;

        // Munculkan musuh secara berkala
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval; // Reset timer
        }
    }

    private void SpawnEnemy()
    {
        // Pengaman jika prefab belum dimasukkan di Inspector
        if (enemyPrefab == null) return;

        // LOGIKA RANDOM POSISI: Memilih tempat spawn secara acak dari list spawnPoints
        Vector3 spawnPos = transform.position; // Default jika spawnPoints kosong
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            spawnPos = spawnPoints[randomIndex].position;
        }

        // LANGSUNG INSTANTIATE: Lahirkan musuh ke dunia game
        Instantiate(enemyPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity);
    }
}