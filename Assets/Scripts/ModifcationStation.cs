using UnityEngine;
using UnityEngine.UI;

public class ModifcationStation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;

    [Header("Progress Settings")]
    [SerializeField] private float secondsToActivate = 5f;

    private float activeSeconds = 0f;
    private bool playerInside = false;

    private void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0;
            progressSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleProgress();
    }

    private void HandleProgress()
    {
        if (playerInside)
        {
            activeSeconds += Time.deltaTime;
            float progress = Mathf.Clamp01(activeSeconds / secondsToActivate);

            if (progressSlider != null)
                progressSlider.value = progress;

            if (progress >= 1f)
            {
                Debug.Log("APPLY MODIFICATION UPGRADE SCREEN");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player Inside");
            progressSlider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player Left");
        }
    }
}
