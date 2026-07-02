using UnityEngine;
using TMPro; // Wajib dimasukkan untuk mengontrol TextMeshPro

public class ScoreManager : MonoBehaviour
{
    // Instance static agar skrip lain (seperti musuh) bisa menambah poin dengan mudah
    public static ScoreManager Instance { get; private set; }

    [Header("Referensi UI")]
    public TextMeshProUGUI scoreText; // Tarik objek ScoreText ke sini di Inspector

    private int currentScore = 0;

    void Awake()
    {
        // Setup Singleton Pattern
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI();
    }

    // Fungsi yang akan dipanggil oleh skrip lain untuk menambah poin
    public void AddScore(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        UpdateScoreUI();
    }

    // Fungsi internal untuk memperbarui teks di layar
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    // Fungsi opsional untuk reset score (misal saat player restart/respawn)
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }
}