using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnStartClick()
    {
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene("Gameplay");    
    }

    public void OnExitClick()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
    }
}
