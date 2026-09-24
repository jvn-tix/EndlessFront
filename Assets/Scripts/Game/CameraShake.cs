using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Mengambil komponen ImpulseSource yang ada di objek ini
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Memanggil guncangan kamera menggunakan Cinemachine Impulse.
    /// </summary>
    /// <param name="force">Kekuatan guncangan (default: 1.0f)</param>
    public void Shake(float force = 1.0f)
    {
        if (impulseSource != null)
        {
            // Memicu guncangan kamera dengan kekuatan tertentu
            impulseSource.GenerateImpulseWithForce(force);
        }
    }
}