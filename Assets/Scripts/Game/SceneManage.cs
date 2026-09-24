using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private Slider progressBar;
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float progressSpeed = 2f; 

    public void OnStartClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM();
        }

        StartCoroutine(LoadSceneAsyncRoutine("Gameplay"));
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        if (loadingCanvasGroup == null)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        // Reset slider ke 0 saat mulai
        if (progressBar != null) progressBar.value = 0f;

        // 1. Fade In Loading Panel
        loadingCanvasGroup.gameObject.SetActive(true);
        loadingCanvasGroup.blocksRaycasts = true;

        while (loadingCanvasGroup.alpha < 1f)
        {
            loadingCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        loadingCanvasGroup.alpha = 1f;

        // 2. Load Scene Async
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Tahan scene baru dulu

        float targetProgress = 0f;

        while (!operation.isDone)
        {
            // Ambil target progress asli dari Unity (0.0 sampai 1.0)
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Gerakkan Slider secara mulus (smooth interpolation) menuju target
            if (progressBar != null)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, targetProgress, Time.deltaTime * progressSpeed);
            }

            // Jika proses loading Unity selesai (0.9) DAN slider visual sudah penuh (1.0)
            if (operation.progress >= 0.9f && (progressBar == null || progressBar.value >= 0.99f))
            {
                // Izinkan masuk ke scene Gameplay
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}