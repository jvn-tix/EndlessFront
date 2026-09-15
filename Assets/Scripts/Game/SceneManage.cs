using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void OnStartClick()
    {
        Debug.Log("Start button clicked!");
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM(); // Stop the current BGM
        }
        SceneManager.LoadScene("Gameplay");    
    }

    public void OnExitClick()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
    }
}
