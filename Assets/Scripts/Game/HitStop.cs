using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }

    private bool isFreezing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Menghentikan waktu game secara mikro.
    /// </summary>
    /// <param name="duration">Durasi freeze dalam hitungan detik nyata (default: 0.04 detik)</param>
    public void Freeze(float duration = 0.04f)
    {
        if (isFreezing) return;
        StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        isFreezing = true;

        // Simpan timeScale asli (biasanya 1f)
        float originalTimeScale = Time.timeScale;

        // Hentikan waktu game
        Time.timeScale = 0f;

        // Tunggu menggunakan detik nyata (Realtime) karena Time.timeScale sedang 0
        yield return new WaitForSecondsRealtime(duration);

        // Kembalikan waktu ke normal
        Time.timeScale = originalTimeScale;
        isFreezing = false;
    }
}