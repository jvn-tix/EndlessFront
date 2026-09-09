using UnityEngine;
using UnityEngine.InputSystem; // Tetap wajib impor namespace New Input System
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels & Buttons")]
    public GameObject pauseMenuPanel;
    public GameObject pauseButton;

    public static bool isPaused = false;

    void Update()
    {
        // Langsung cek status tombol Escape lewat New Input System
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);

        Time.timeScale = 0f; // Menghentikan waktu permainan
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);

        Time.timeScale = 1f; // Menjalankan kembali waktu
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("HomeMenu");
    }
}