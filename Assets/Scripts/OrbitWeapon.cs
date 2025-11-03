using UnityEngine;

public class OrbitWeapon : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] private float orbitSpeed = 90f;
    [SerializeField] private float orbitRadius = 2.0f;

    private Transform playerTransform;
    private Transform spriteChild;

    private void Start()
    {
        playerTransform = transform.parent;
        spriteChild = transform.GetChild(0);

        spriteChild.localPosition = new Vector3(orbitRadius, 0f, 0f);
    }

    private void Update()
    {
        transform.RotateAround(playerTransform.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }
}
