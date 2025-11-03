using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    [Header("Target and Bounds")]
    public Transform target;
    public EdgeCollider2D boundsCollider;

    private Camera cam;
    private float camHeight;
    private float camWidth;

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

        // Desired camera position (following the player)
        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Clamp X only if needed
        if (desiredPos.x < minX) desiredPos.x = minX;
        if (desiredPos.x > maxX) desiredPos.x = maxX;

        // Clamp Y only if needed
        if (desiredPos.y < minY) desiredPos.y = minY;
        if (desiredPos.y > maxY) desiredPos.y = maxY;

        // Smoothly move camera
        transform.position = desiredPos;
    }
}
