using System.Collections;
using System.Collections.Generic;
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
    private List<GameObject> playersInScene;
    void Start()
    {
        playersInScene = new List<GameObject>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        restart.onClick.AddListener(RestartGame);
        exit.onClick.AddListener(ExitGame);
        StartCoroutine(FindAlivePlayers());
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

    private IEnumerator FindAlivePlayers()
    {
        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
            yield break;

        while (true)
        {
            GameObject[] auxPlayers = GameObject.FindGameObjectsWithTag("Player");
            playersInScene.Clear();

            foreach (GameObject player in auxPlayers)
            {
                Player p = player.GetComponent<Player>();
                if (p != null && p.currentState == Player.PlayerState.ALIVE)
                {
                    playersInScene.Add(player);
                }
            }

            if (playersInScene.Count == 0)
            {
                ShowMenu();
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowGameOverForAll()
    {
        ShowMenu();
    }

    void RestartGame()
    {
        if (!PhotonNetwork.InRoom)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ForceRestartGameOver", RpcTarget.All, photonView.ViewID);
        }
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
    public void ForceRestartGameOver(int viewId){
        if(photonView.ViewID == viewId)
        {    
            SceneManager.LoadScene("GameOnline");
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
