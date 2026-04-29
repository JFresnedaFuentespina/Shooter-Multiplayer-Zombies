using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button play;
    public Button exit;
    public Button multiplayer;
    public NetworkingManager networkingManager;
    void Start()
    {
        play.onClick.AddListener(RestartGame);
        exit.onClick.AddListener(ExitGame);
        multiplayer.onClick.AddListener(LoadMultiplayerMenu);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    void LoadMultiplayerMenu()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    void ExitGame()
    {
        Application.Quit(); // Exit the game
    }
}
