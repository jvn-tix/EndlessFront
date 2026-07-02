using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthPrefab;
    public Sprite fullHealthSprite;
    public Sprite emptyHealthSprite;

    private List<Image> hearts = new List<Image>();
   
    public void setMaxHearts(int maxHearts)
    {
        // Hapus semua hati yang ada sebelumnya
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();
        // Buat hati baru sesuai maxHearts
        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(healthPrefab, transform);
            newHeart.sprite = fullHealthSprite;
            newHeart.color = Color.red;
            hearts.Add(newHeart);
        }
    }

    public void updateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHealthSprite;
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].sprite = emptyHealthSprite;
                hearts[i].color = Color.white;
            }
        }
    }
}
