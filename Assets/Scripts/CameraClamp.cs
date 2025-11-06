using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    [Header("Target and Bounds")]
    [SerializeField] private Transform target;
    [SerializeField] private EdgeCollider2D boundsCollider;
    
    private Camera cam;
    private float camHeight;
    private float camWidth;

    // Allow for shake
    private Vector3 shakeOffset = Vector3.zero;

    void Awake()
    {
        cam = GetComponent<Camera>();
        camHeight = cam.orthographicSize * 2f;
        camWidth = camHeight * cam.aspect;
    }

    void LateUpdate()
    {
        if (target == null || boundsCollider == null) return;

        Bounds bounds = boundsCollider.bounds;

        // Compute min/max camera center positions
        float minX = bounds.min.x + camWidth / 2f;
        float maxX = bounds.max.x - camWidth / 2f;
        float minY = bounds.min.y + camHeight / 2f;
        float maxY = bounds.max.y - camHeight / 2f;

        // Desired camera position (following the player) accounting for shake
        Vector3 desiredPos = new Vector3(target.position.x + shakeOffset.x, target.position.y + shakeOffset.y, transform.position.z);

        // Clamp camera position within bounds
        if (desiredPos.x < minX) desiredPos.x = minX;
        if (desiredPos.x > maxX) desiredPos.x = maxX;
        if (desiredPos.y < minY) desiredPos.y = minY;
        if (desiredPos.y > maxY) desiredPos.y = maxY;

        // Move the camera to the position
        transform.position = desiredPos;
    }

    public void SetShake(Vector2 offset) => shakeOffset = offset;
    public void ClearShake() => shakeOffset = Vector2.zero;
}
