using UnityEngine;
using UnityEngine.InputSystem; // Pastikan ini tetap ada di paling atas

public class PlayerSight : MonoBehaviour
{
    void Update()
    {
        // 1. Ambil posisi piksel mouse dari New Input System
        Vector2 mousePixelPos = Mouse.current.position.ReadValue();

        // 2. Konversi ke posisi dunia game 2D
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePixelPos.x, mousePixelPos.y, 10f));

        // 3. Hitung vektor arah dari GunPivot ke posisi posisi Mouse
        Vector2 directionToMouse = mouseWorldPos - transform.position;

        // 4. Hitung sudut dalam derajat
        float targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // 5. KUNCI KE 4 ARAH (Kelipatan 90 derajat)
        float snappedAngle = Mathf.Round(targetAngle / 90f) * 90f;

        // 6. Putar GunPivot berdasarkan sumbu Z
        transform.rotation = Quaternion.Euler(0, 0, snappedAngle);
    }
}