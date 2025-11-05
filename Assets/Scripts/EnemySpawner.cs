using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private GameObject spawnMarkerPrefab;
    [SerializeField] private EdgeCollider2D gameBounds;
    [SerializeField] private Camera mainCamera;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int numberToSpawn = 1;

    [Header("Difficulty Handler")]
    [SerializeField] private DifficultyHandler difficultyHandler;

    private float minX, maxX, minY, maxY;

    // Debug testing spawns
    private float spawnTimer = 0f;

    private void Awake()
    { 
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Calculate bounds from EdgeCollider2D
        Vector2[] points = gameBounds.points;
        minX = points[0].x;
        maxX = points[0].x;
        minY = points[0].y;
        maxY = points[0].y;

        foreach (Vector2 point in points)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.y > maxY) maxY = point.y;
        }
    }

    private void Update()
    {
        CalculateSpawnRatesWithDifficulty(difficultyHandler.CurrentDifficulty);

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemies(numberToSpawn);
            spawnTimer = 0f;
        }
    }

    private void SpawnEnemies(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector2 spawnPosition = GetRandomPositionInVisibleBounds();

            GameObject markerObj = Instantiate(spawnMarkerPrefab, spawnPosition, Quaternion.identity);

            StartCoroutine(SpawnEnemyAfterDelay(spawnPosition, spawnDelay, markerObj));
        }
    }

    private Vector2 GetRandomPositionInVisibleBounds()
    {
        // Calculating camera spawn areas
        float zDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 camBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 camTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        // Overlapping the camera bounds within the playable game bounds
        float visibleMinX = Mathf.Max(minX, camBottomLeft.x);
        float visibleMaxX = Mathf.Min(maxX, camTopRight.x);
        float visibleMinY = Mathf.Max(minY, camBottomLeft.y);
        float visibleMaxY = Mathf.Min(maxY, camTopRight.y);

        // If no overlap, return center of game bounds as a fallback
        if (visibleMinX >= visibleMaxX || visibleMinY >= visibleMaxY)
        {
            float centerX = (minX + maxX) * 0.5f;
            float centerY = (minY + maxY) * 0.5f;
            return new Vector2(centerX, centerY);
        }

        float x = Random.Range(visibleMinX, visibleMaxX);
        float y = Random.Range(visibleMinY, visibleMaxY);

        return new Vector2(x, y);
    }

    IEnumerator SpawnEnemyAfterDelay(Vector2 position, float delay, GameObject marker)
    {
        yield return new WaitForSeconds(delay);
        GameObject enemyObj = Instantiate(enemyPrefab, position, Quaternion.identity, enemiesParent);
        Destroy(marker);
    }

    private void CalculateSpawnRatesWithDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                spawnDelay = 1.5f;
                spawnInterval = 1f;
                numberToSpawn = 1;
                break;
            case Difficulty.MEDIUM:
                spawnDelay = 1f;
                spawnInterval = 0.8f;
                numberToSpawn = 2;
                break;
            case Difficulty.HARD:
                spawnDelay = 1f;
                spawnInterval = 0.6f;
                numberToSpawn = 2;
                break;
            case Difficulty.EXTREME:
                spawnDelay = 0.75f;
                spawnInterval = 0.6f;
                numberToSpawn = 3;
                break;
            case Difficulty.INSANITY:
                spawnDelay = 0.5f;
                spawnInterval = 0.5f;
                numberToSpawn = 5;
                break;
        }
    }
}
