using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI highScoreText;

    public void ShowGameOverUI()
    {
        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CheckandSaveHighScore();

            int highScore = ScoreManager.Instance.GetHighScore();

            if(highScoreText != null)
            {
                highScoreText.text = "High Score: " + highScore;
            }
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("HomeMenu");
    }
}
