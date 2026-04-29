using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (!PhotonNetwork.InRoom)
            Time.timeScale = 0f; // Pause the game

        pauseMenu.SetActive(true); // Show the pause menu
        isPaused = true;
    }

    void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!PhotonNetwork.InRoom)
            Time.timeScale = 1f; // Resume the game

        pauseMenu.SetActive(false); // Hide the pause menu
        isPaused = false;
    }

    void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
