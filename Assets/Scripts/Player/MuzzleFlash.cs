using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] Light2D flashLight;
    [SerializeField] float duration = 0.06f;

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        flashLight.enabled = true;
        yield return new WaitForSeconds(duration);
        flashLight.enabled = false;
    }
}