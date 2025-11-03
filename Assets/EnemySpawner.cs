using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EdgeCollider2D gameBounds;
    [SerializeField] private Transform enemiesParent;

    [Header("Spawn Marker Settings")]
    [SerializeField] private GameObject spawnMarkerPrefab;
    [SerializeField] private float spawnDelay = 1f;

    private float minX, maxX, minY, maxY;

    // Debug testing spawns
    private float spawnInterval = 1f;
    private float spawnTimer = 0f;

    private void Awake()
    {
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
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    public void SpawnEnemy()
    {
        float spawnX = Random.Range(minX, maxX);
        float spawnY = Random.Range(minY, maxY);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        GameObject markerObj = Instantiate(spawnMarkerPrefab, spawnPosition, Quaternion.identity);

        StartCoroutine(SpawnEnemyAfterDelay(spawnPosition, spawnDelay, markerObj));
    }

    IEnumerator SpawnEnemyAfterDelay(Vector2 position, float delay, GameObject marker)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(enemyPrefab, position, Quaternion.identity, enemiesParent);
        Destroy(marker);
    }
}
