using UnityEngine;
using UnityEngine.UI;

public class ModifcationStation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private FloatingText floatingText;
    [SerializeField] private AudioClip sfx;

    [Header("Progress Settings")]
    [SerializeField] private float secondsToActivate = 5f;
    [SerializeField] private int minLevel = 5;
    [SerializeField] private int maxLevel = 6;

    private float activeSeconds = 0f;
    private bool playerInside = false;
    private bool activatable = false;
    private bool isActivated = false;

    private PlayerStatsHandler playerStats;
    private ModificationPanelController modificationPanelController;

    private Canvas worldCanvas;

    private void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0;
            progressSlider.gameObject.SetActive(false);
        }
        playerStats = FindFirstObjectByType<PlayerStatsHandler>();
        modificationPanelController = FindFirstObjectByType<ModificationPanelController>();
        worldCanvas = GameObject.Find("WorldCanvas").GetComponent<Canvas>();
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

            if (progress >= 1f && !isActivated)
            {
                modificationPanelController.ShowPanel();

                // Audio clip
                AudioManager.Instance.PlaySFX(sfx);

                // Floating mod gained text
                var modText = Instantiate(floatingText, worldCanvas.transform);
                Vector3 offset = new Vector3(0f, 1f, 0f);
                modText.SetText("MOD GAINED!");
                modText.transform.position = transform.position + offset;

                isActivated = true;
            }

            // Clear up after activation
            if (isActivated)
                Destroy(gameObject);
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
