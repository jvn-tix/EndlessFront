using UnityEngine;
using UnityEngine.InputSystem; // Pastikan ini tetap ada di paling atas

public class PlayerSight : MonoBehaviour
{
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform.parent; // Asumsikan PlayerSight adalah anak dari Player
    }
    void Update()
    {
       float facingDirection = Mathf.Sign(playerTransform.localScale.x);
        float targetAngle = 0f;
        if (facingDirection > 0)
        {
            targetAngle = 0f; // Menghadap kanan
        }
        else if (facingDirection < 0)
        {
            targetAngle = 180f; // Menghadap kiri
        }
        transform.localRotation = Quaternion.Euler(0f, 0f, targetAngle);
    }
}
