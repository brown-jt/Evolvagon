using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3.0f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("References")]
    [SerializeField] private AudioClip pickupSfx;

    private Vector2 moveDirection;
    private Camera cam;
    private readonly float offscreenDistance = 1f;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.Translate(movementSpeed * Time.deltaTime * moveDirection);
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (IsOffScreen())
            Destroy(gameObject);
    }

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private bool IsOffScreen()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        return viewPos.x < -offscreenDistance ||
               viewPos.x > 1 + offscreenDistance ||
               viewPos.y < -offscreenDistance ||
               viewPos.y > 1 + offscreenDistance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Upgrade star weapon
            AudioManager.Instance.PlaySFX(pickupSfx);
            Destroy(gameObject);
        }
    }
}
