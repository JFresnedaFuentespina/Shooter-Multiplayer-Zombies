using System.Collections;
using Photon.Pun;
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
    public PhotonView photonView;
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

        if (!PhotonNetwork.InRoom)
            Time.timeScale = 0f;

        isPaused = true;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ExitGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            photonView.RPC("ForceExitToMenuGameOver", RpcTarget.All, photonView.ViewID);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    
    [PunRPC]
    public void ForceExitToMenuGameOver(int viewId)
    {
        if (photonView.ViewID == viewId)
        {
            StartCoroutine(WaitForLeavingRoom());
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator WaitForLeavingRoom()
    {
        PhotonNetwork.LeaveRoom();
        // PhotonNetwork.Disconnect();

        while (PhotonNetwork.InRoom)
            yield return null;
    }
}
