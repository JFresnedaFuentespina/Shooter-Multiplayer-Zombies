using Photon.Pun;
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        play.onClick.AddListener(LoadSinglePlayer);
        exit.onClick.AddListener(ExitGame);
        multiplayer.onClick.AddListener(LoadMultiplayerMenu);
    }

    void LoadSinglePlayer()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        SceneManager.LoadScene("Game");
    }

    void LoadMultiplayerMenu()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        SceneManager.LoadScene("MultiplayerMenu");
    }

    void ExitGame()
    {
        Application.Quit(); // Exit the game
    }
}
