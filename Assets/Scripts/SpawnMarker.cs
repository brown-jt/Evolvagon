using UnityEngine;

public class SpawnMarker : MonoBehaviour
{
    [Header("Marker Settings")]
    [SerializeField] private float minScale = 1.0f;
    [SerializeField] private float maxScale = 2.0f;
    [SerializeField] private float flashSpeed = 5f;
    [SerializeField] private float pulseSpeed = 5f;

    private SpriteRenderer sr;
    private float timer = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Flash color between red and white
        sr.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(timer * flashSpeed, 1f));

        // Pulse scale between min and max
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(timer * pulseSpeed * Mathf.PI * 2) + 1f) / 2f);
        transform.localScale = Vector3.one * scale;
    }
}
