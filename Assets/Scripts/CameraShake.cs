using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CameraClamp cameraClamp;

    void Awake()
    {
        cameraClamp = Camera.main.GetComponent<CameraClamp>();
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Only advance elapsed when the game is not paused
            if (Time.timeScale > 0f)
            {
                elapsed += Time.deltaTime;

                // Random shake offset
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                cameraClamp.SetShake(new Vector2(x, y));
            }

            yield return null; // wait for next frame
        }

        // Clear shake after finishing
        cameraClamp.ClearShake();
    }
}
