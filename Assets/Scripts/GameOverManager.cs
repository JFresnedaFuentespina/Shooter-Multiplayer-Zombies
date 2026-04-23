using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject gameOverMenu;
    public Button restart;
    public Button exit;
    public bool isPaused = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        restart.onClick.AddListener(RestartGame);
        exit.onClick.AddListener(ExitGame);
    }

    public void ShowMenu()
    {
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
