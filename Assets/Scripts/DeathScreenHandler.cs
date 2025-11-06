using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class DeathScreenHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DifficultyHandler difficultyHandler;
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private Canvas canvasUI;
    [Space(10)]
    [SerializeField] private AudioClip deathMusic;
    [SerializeField] private AudioClip gameMusic;
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
        AudioManager.Instance.PlayMusic(gameMusic);
        deathScreenPanel.SetActive(false);
        deathScreenVisible = false;
        canvasUI.gameObject.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void ShowDeathScreen()
    {
        AudioManager.Instance.PlayMusic(deathMusic);
        deathScreenPanel.SetActive(true);
        SetText();
        deathScreenVisible = true;
        canvasUI.gameObject.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("TitleScene");
        AudioManager.Instance.PlayMusic(gameMusic);
    }

    private void SetText()
    {
        time.text = baseTimeText + FormatTime(difficultyHandler.ElapsedTime);
        score.text = baseScoreText + gameManager.GameScore;
    }

    private void CheckForSuperSecretCheatCode()
    {
        if (keyboard == null) return;

        foreach (KeyControl key in keyboard.allKeys)
        {
            if (key == null)
                continue;

            if (key.wasPressedThisFrame)
            {
                string pressed = key.displayName.ToLower();

                // Accept letters only
                if (pressed.Length == 1 && char.IsLetter(pressed[0]))
                {
                    char expectedChar = superSecretCheatCode[superSecretInput.Length];

                    if (pressed[0] == expectedChar)
                    {
                        // Correct letter in sequence → add it
                        superSecretInput += pressed;

                        // Sequence complete?
                        if (superSecretInput == superSecretCheatCode)
                        {
                            ActivateSuperSecretCheatCode();
                            superSecretInput = "";     // reset after activation
                        }
                    }
                    else
                    {
                        // WRONG LETTER → reset entire input
                        superSecretInput = "";
                    }
                }
                else
                {
                    // Non-letter keys reset the input
                    superSecretInput = "";
                }
            }
        }
    }

    private void ActivateSuperSecretCheatCode()
    {
        playerStats.Heal(5);
        HideDeathScreen();
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0}:{1:00}", minutes, seconds);
    }
}
