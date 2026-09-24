using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Referensi Anchor Fill")]
    [Tooltip("Tarik GameObject 'FillAnchor' ke sini")]
    [SerializeField] private Transform fillAnchor;

    private Vector3 originalScale;
    private bool isInitialized = false;

    private void Awake()
    {
        InitScale();
    }

    private void InitScale()
    {
        if (!isInitialized && fillAnchor != null)
        {
            originalScale = fillAnchor.localScale;
            isInitialized = true;
        }
    }
    private void Update()
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (fillAnchor == null) return;

        InitScale();

        // Hitung rasio HP (0.0 - 1.0)
        float healthPercent = Mathf.Clamp01((float)currentHealth / maxHealth);

        // Ubah skala X milik FillAnchor
        fillAnchor.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }
}