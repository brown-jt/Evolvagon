using System.Collections;
using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    private float expAmount;
    private PlayerStatsHandler playerStats;
    private bool isCollected = false;

    private void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStatsHandler>();    
    }

    public void Collect(Transform player)
    {
        if (isCollected) return;
        isCollected = true;

        StartCoroutine(CollectRoutine(player));
    }

    public void SetExperience(float exp)
    {
        expAmount = exp;
    }

    private IEnumerator CollectRoutine(Transform player)
    {
        // Jump up
        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * 0.4f;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            transform.position = Vector3.Lerp(startPos, peakPos, t);
            yield return null;
        }

        // Land back down
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            transform.position = Vector3.Lerp(peakPos, startPos, t);
            yield return null;
        }

        // Fly towards player
        float speed = 0.2f;
        float maxSpeed = 20f;

        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards
            (
                transform.position,
                player.position,
                Time.deltaTime * speed
            );

            speed = Mathf.Min(speed * 1.1f, maxSpeed);
            yield return null;
        }

        // Add XP 
        playerStats.AddExperience(expAmount);

        // Destroy pickup
        Destroy(gameObject);
    }
}
