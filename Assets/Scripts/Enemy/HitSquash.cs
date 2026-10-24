using System.Collections;
using UnityEngine;

public class HitSquash : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform visual;          // kosongkan = pakai transform sendiri

    [Header("Squash")]
    [SerializeField] float squashX = 0.7f;      // 1 = normal, makin kecil makin gepeng
    [SerializeField] float stretchY = 1.15f;
    [SerializeField] float squashTime = 0.04f;  // fase menekan (cepat)
    [SerializeField] float recoverTime = 0.12f; // fase kembali

    Vector3 originalScale;
    Coroutine routine;

    void Awake()
    {
        if (visual == null) visual = transform;
        originalScale = visual.localScale;
    }

    void OnEnable()
    {
        // penting untuk object pooling: reset scale tiap diaktifkan
        if (visual != null) visual.localScale = originalScale;
    }

    public void Play()
    {
        if (!gameObject.activeInHierarchy) return;
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(SquashRoutine());
    }

    IEnumerator SquashRoutine()
    {
        // pertahankan tanda scale (misal X negatif saat menghadap kiri)
        float signX = Mathf.Sign(originalScale.x);
        Vector3 squashed = new Vector3(
            originalScale.x * squashX,
            originalScale.y * stretchY,
            originalScale.z);

        // 1. menekan cepat
        float t = 0f;
        while (t < squashTime)
        {
            t += Time.unscaledDeltaTime;
            visual.localScale = Vector3.Lerp(originalScale, squashed, t / squashTime);
            yield return null;
        }

        // 2. kembali halus
        t = 0f;
        while (t < recoverTime)
        {
            t += Time.unscaledDeltaTime;
            float k = t / recoverTime;
            k = 1f - (1f - k) * (1f - k); // ease-out
            visual.localScale = Vector3.Lerp(squashed, originalScale, k);
            yield return null;
        }

        visual.localScale = originalScale;
        routine = null;
    }
}