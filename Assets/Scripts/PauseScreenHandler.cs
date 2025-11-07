using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScreenHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseScreenPanel;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private Canvas canvasUI;
    [SerializeField] private InputActionReference pauseActionReference;

    private bool gamePaused = false;

    private void Start()
    {
        HidePauseScreen();
    }
    private void OnEnable()
    {
        pauseActionReference.action.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        pauseActionReference.action.performed -= OnPausePressed;
    }
    
    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (!gamePaused)
        {
            ShowPauseScreen();
        }
    }

    private void HidePauseScreen()
    {
        gamePaused = false;

        AudioManager.Instance.MusicSource.Play();
        pauseScreenPanel.SetActive(false);
        canvasUI.gameObject.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void ShowPauseScreen()
    {
        gamePaused = true;

        AudioManager.Instance.MusicSource.Stop();
        pauseScreenPanel.SetActive(true);
        canvasUI.gameObject.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        HidePauseScreen();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("TitleScene");
        AudioManager.Instance.SetBGMusicPitch(1f);
        AudioManager.Instance.PlayMusic(menuMusic);
        AudioManager.Instance.MusicSource.Play();
    }
}
