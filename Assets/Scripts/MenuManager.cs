using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button play;
    public Button exit;
    void Start()
    {
        play.onClick.AddListener(RestartGame);
        exit.onClick.AddListener(ExitGame);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void ExitGame()
    {
        Application.Quit(); // Exit the game
    }
}
