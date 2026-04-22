using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject pauseMenu;
    public Button resume;
    public Button exit;
    public bool isPaused = false;
    void Start()
    {
        resume.onClick.AddListener(ResumeGame);
        exit.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true); // Show the pause menu
        isPaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false); // Hide the pause menu
        isPaused = false;
    }

    void ExitGame()
    {
        Application.Quit(); // Exit the game
    }
}
