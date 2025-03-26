using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1;
        MenuManager.Instance.OnMenuOpen += ClosePauseMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            ClosePauseMenu();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void OpenPauseMenu()
    {
        MenuManager.Instance.MenuEvent();
        isPaused = true;
        pauseMenu?.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        MenuManager.Instance.MenuEventClosed();
        isPaused = false;
        pauseMenu?.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
