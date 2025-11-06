using UnityEngine;
using UnityEngine.UI;

public class ModifcationStation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;

    [Header("Progress Settings")]
    [SerializeField] private float secondsToActivate = 5f;
    [SerializeField] private int minLevel = 5;
    [SerializeField] private int maxLevel = 6;

    private float activeSeconds = 0f;
    private bool playerInside = false;
    private bool activatable = false;

    private PlayerStatsHandler playerStats;
    private ModificationPanelController modificationPanelController;

    private void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0;
            progressSlider.gameObject.SetActive(false);
        }
        playerStats = FindFirstObjectByType<PlayerStatsHandler>();
        modificationPanelController = FindFirstObjectByType<ModificationPanelController>();
        
        if (modificationPanelController != null )
            modificationPanelController.HidePanel();
    }

    private void Update()
    {
        CheckIfCorrectLevel();
    }

    private void HandleProgress()
    {
        if (playerInside && activatable)
        {
            // If not visible already let's make it visible
            progressSlider.gameObject.SetActive(true);

            activeSeconds += Time.deltaTime;
            float progress = Mathf.Clamp01(activeSeconds / secondsToActivate);

            if (progressSlider != null)
                progressSlider.value = progress;

            if (progress >= 1f && !modificationPanelController.IsVisible)
            {
                modificationPanelController.ShowPanel();
            }
        }
    }

    private void CheckIfCorrectLevel()
    {
        if (playerStats.Level < minLevel)
        {
            activatable = false;
        }
        if (playerStats.Level >= minLevel && playerStats.Level <= maxLevel) 
        {
            activatable = true;
        }
        if (playerStats.Level > maxLevel)
        {
            Destroy(gameObject);
        }

        HandleProgress();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
