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
        multiplayer.interactable = false;
        play.onClick.AddListener(RestartGame);
        exit.onClick.AddListener(ExitGame);
        multiplayer.onClick.AddListener(Multiplayer);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void Multiplayer()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void ExitGame()
    {
        Application.Quit(); // Exit the game
    }
}
