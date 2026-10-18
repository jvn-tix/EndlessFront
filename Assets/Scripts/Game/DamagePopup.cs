using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float moveSpeedY = 1.5f;
    private float disappearTimer = 0.5f;
    private Color textColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount)
    {
        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
            textColor = textMesh.color;
        }

        StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        float timer = disappearTimer;

        while (timer > 0)
        {
            // Melayang ke atas
            transform.position += new Vector3(0, moveSpeedY * Time.deltaTime, 0);

            // Memudar secara bertahap (Fade Out)
            timer -= Time.deltaTime;
            if (timer < disappearTimer * 0.5f)
            {
                textColor.a -= (1f / (disappearTimer * 0.5f)) * Time.deltaTime;
                if (textMesh != null) textMesh.color = textColor;
            }

            yield return null;
        }

        // Hapus otomatis setelah memudar
        Destroy(gameObject);
    }
}