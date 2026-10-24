using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampFlicker : MonoBehaviour
{
    [Header("Glow (bola bohlam)")]
    [SerializeField] SpriteRenderer glow;
    [SerializeField, Range(0f, 1f)] float baseAlpha = 0.3f;
    [SerializeField, Range(0f, 1f)] float alphaAmount = 0.2f;

    [Header("Light (cone)")]
    [SerializeField] Light2D lampLight;
    [SerializeField] float baseIntensity = 1.2f;
    [SerializeField] float intensityAmount = 0.5f;

    [Header("Umum")]
    [SerializeField] float speed = 8f;
    [SerializeField] bool brokenBulb = false; // centang untuk efek bohlam rusak

    float offset;

    void Start()
    {
        offset = Random.value * 100f;
    }

    void Update()
    {
        // Perlin noise: hasil -1 sampai 1, acak dan halus
        float wave = Mathf.PerlinNoise(Time.time * speed, offset) * 2f - 1f;

        // Opsional: sesekali mati mendadak
        if (brokenBulb && Mathf.PerlinNoise(Time.time * 1.5f, offset + 50f) > 0.85f)
            wave = -1f;

        if (glow != null)
        {
            var c = glow.color;
            c.a = Mathf.Clamp01(baseAlpha + wave * alphaAmount);
            glow.color = c;
        }

        if (lampLight != null)
            lampLight.intensity = Mathf.Max(0f, baseIntensity + wave * intensityAmount);
    }
}