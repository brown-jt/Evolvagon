using System.Collections;
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
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private AudioClip correctSfx;
    [SerializeField] private AudioClip wrongSfx;
    [SerializeField] private AudioClip completeSfx;
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
        AudioManager.Instance.MusicSource.Play();
        deathScreenPanel.SetActive(false);
        deathScreenVisible = false;
        canvasUI.gameObject.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void ShowDeathScreen()
    {
        AudioManager.Instance.MusicSource.Stop();
        AudioManager.Instance.SFXSource.PlayOneShot(deathSfx);
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
        AudioManager.Instance.MusicSource.Play();
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

                // Since player is using WASD to move let's not accept it as it will cause suspicion if pressed after death
                if (pressed == "w" || pressed == "a" || pressed == "s" || pressed == "d")
                    continue;

                // Accept letters only
                if (pressed.Length == 1 && char.IsLetter(pressed[0]))
                {
                    char expectedChar = superSecretCheatCode[superSecretInput.Length];

                    if (pressed[0] == expectedChar)
                    {
                        // Correct letter in sequence → add it
                        superSecretInput += pressed;
                        AudioManager.Instance.SFXSource.PlayOneShot(correctSfx);

                        // Sequence complete?
                        if (superSecretInput == superSecretCheatCode)
                        {
                            StartCoroutine(ActivateSuperSecretCheatCodeRoutine());
                        }
                    }
                    else
                    {
                        // WRONG LETTER → reset entire input
                        superSecretInput = "";
                        AudioManager.Instance.SFXSource.PlayOneShot(wrongSfx);
                    }
                }
                else
                {
                    // Non-letter keys reset the input
                    superSecretInput = "";
                    AudioManager.Instance.SFXSource.PlayOneShot(wrongSfx);
                }
            }
        }
    }

    private IEnumerator ActivateSuperSecretCheatCodeRoutine()
    {
        AudioManager.Instance.SFXSource.PlayOneShot(completeSfx);
        superSecretInput = "";

        yield return new WaitForSecondsRealtime(0.75f);

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
