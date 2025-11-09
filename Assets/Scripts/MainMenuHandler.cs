using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject keybindsMenu;

    private readonly string gameSceneName = "OnlyScene";

    private void Start()
    {
        settingsMenu.SetActive(false);
        audioMenu.SetActive(false);
        keybindsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void GoToSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void GoToAudio()
    {
        audioMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void GoToKeybinds()
    {
        keybindsMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void GoBackToSettings()
    {
        keybindsMenu.SetActive(false);
        audioMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void GoToTitle()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
