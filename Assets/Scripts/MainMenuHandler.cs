using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    private readonly string gameSceneName = "OnlyScene";

    private void Start()
    {
        settingsMenu.SetActive(false);
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

    public void GoToTitle()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

        #else
            Application.Quit();

        #endif
    }
}
