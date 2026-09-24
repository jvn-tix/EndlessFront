using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameplayMenuUI: MonoBehaviour
{
    [Header("Pause UI References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject pauseButton;

    [Header("Game Over UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Transition / Loading References (Opsional)")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private float fadeSpeed = 3f;

    public static bool isPaused = false;

    private void Start()
    {
        // Pastikan timeScale direset saat scene Gameplay baru dimulai
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Update()
    {
        // Deteksi tombol Escape untuk Pause/Resume
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    // ==================== PAUSE SYSTEM ====================

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;
    }

    // ==================== GAME OVER SYSTEM ====================

    public void ShowGameOverUI()
    {
        if(gameOverPanel != null) gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CheckandSaveHighScore();
            int highScore = ScoreManager.Instance.GetHighScore();

            if (highScoreText != null)
            {
                highScoreText.text = "High Score: " + highScore;
            }
        }
    }

    // ==================== NAVIGATION / SCENE LOAD ====================

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        StartCoroutine(LoadSceneRoutine("HomeMenu"));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Jika Loading Panel di-assign, jalankan efek Fade Out
        if (loadingCanvasGroup != null)
        {
            loadingCanvasGroup.gameObject.SetActive(true);
            loadingCanvasGroup.blocksRaycasts = true;

            while (loadingCanvasGroup.alpha < 1f)
            {
                loadingCanvasGroup.alpha += Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }
            loadingCanvasGroup.alpha = 1f;
        }

        SceneManager.LoadScene(sceneName);
    }
}