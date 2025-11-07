using UnityEngine;

public class ShootingStarSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject shootingStarPrefab;
    [SerializeField] private PlayerStatsHandler playerStats;

    private readonly float spawnDistance = 1f;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        if (playerStats != null)
            playerStats.OnLevelChanged += SpawnStar;
    }

    private void OnDestroy()
    {
        if (playerStats != null)
            playerStats.OnLevelChanged -= SpawnStar;
    }

    public void SpawnStar(int _)
    {
        Vector2 spawnPos = GetRandomScreenEdgePosition();
        GameObject newStar = Instantiate(shootingStarPrefab, spawnPos, Quaternion.identity);

        // Assign direction into the screen
        Vector2 screenCenter = cam.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
        Vector2 direction = (screenCenter - spawnPos).normalized;

        newStar.GetComponent<ShootingStar>().Initialize(direction);
    }

    private Vector2 GetRandomScreenEdgePosition()
    {
        float rand = Random.value;

        // Screen corners in world space
        Vector2 bottomLeft = cam.ScreenToWorldPoint(Vector2.zero);
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // Choose edge
        if (rand < 0.25f)
        {
            // Left edge
            float y = Random.Range(bottomLeft.y, topRight.y);
            return new Vector2(bottomLeft.x - spawnDistance, y);
        }
        else if (rand < 0.5f)
        {
            // Right edge
            float y = Random.Range(bottomLeft.y, topRight.y);
            return new Vector2(topRight.x + spawnDistance, y);
        }
        else if (rand < 0.75f)
        {
            // Bottom edge
            float x = Random.Range(bottomLeft.x, topRight.x);
            return new Vector2(x, bottomLeft.y - spawnDistance);
        }
        else
        {
            // Top edge
            float x = Random.Range(bottomLeft.x, topRight.x);
            return new Vector2(x, topRight.y + spawnDistance);
        }
    }
}
