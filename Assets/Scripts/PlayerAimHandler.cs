using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class PlayerAimHandler : MonoBehaviour
{
    private readonly float padding = 32f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        // Get mouse position from the new Input System
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Clamp mouse position to screen bounds minus padding
        mousePos.x = Mathf.Clamp(mousePos.x, padding, Screen.width - padding);
        mousePos.y = Mathf.Clamp(mousePos.y, padding, Screen.height - padding);

        // Convert screen position to world position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0f));

        // Make sure z is 0
        worldPos.z = 0f;

        // Move sprite
        transform.position = worldPos;
    }
}
