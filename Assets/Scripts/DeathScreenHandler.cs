using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathScreenHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DifficultyHandler difficultyHandler;
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private Canvas canvasUI;
    [Space(10)]
    [SerializeField] private GameObject deathScreenPanel;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI score;

    private readonly string baseTimeText = "Time Survived: ";
    private readonly string baseScoreText = "Final Score: ";

    private bool deathScreenVisible = false;

    private string superSecretCheatCode = "gernot";
    private string superSecretInput = "";

    private Keyboard keyboard;

    private void Start()
    {
        HideDeathScreen();

        keyboard = Keyboard.current;

        playerStats.OnPlayerDeath += ShowDeathScreen;
    }

    private void Update()
    {
        if (deathScreenVisible)
        {
            CheckForSuperSecretCheatCode();
        }
    }
    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnPlayerDeath -= ShowDeathScreen;
        }
    }

    private void HideDeathScreen()
    {
        deathScreenPanel.SetActive(false);
        deathScreenVisible = false;
        canvasUI.gameObject.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void ShowDeathScreen()
    {
        deathScreenPanel.SetActive(true);
        SetText();
        deathScreenVisible = true;
        canvasUI.gameObject.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SetText()
    {
        time.text = baseTimeText + FormatTime(difficultyHandler.ElapsedTime);
        score.text = baseScoreText + gameManager.GameScore;
    }

    private void CheckForSuperSecretCheatCode()
    {
        // TODO - New Unity input system check to see if superSecretCheatCode was typed successfully
        // Reset typed word on wrong key input
    }

    private void ActivateSuperSecretCheatCode()
    {
        Debug.Log("TODO - REVIVE PLAYER");
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0}:{1:00}", minutes, seconds);
    }
}
