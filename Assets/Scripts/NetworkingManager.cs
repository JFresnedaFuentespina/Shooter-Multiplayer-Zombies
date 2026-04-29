using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public Button play;
    public Button exit;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(DisconnectPlayer());
        }

        play.onClick.AddListener(FindMatch);
        exit.onClick.AddListener(Exit);

        play.interactable = false;
        Debug.Log("Conectando al servidor...");
        PhotonNetwork.ConnectUsingSettings();
    }

    IEnumerator DisconnectPlayer()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
    }

    void Exit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectando a un lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby!");
        play.interactable = true;
    }

    public void FindMatch()
    {
        Debug.Log("Buscando sala...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se ha encontrado ninguna sala! Creando una sala...");
        MakeRoom();
    }

    private void MakeRoom()
    {
        int randomRoomId = Random.Range(0, 5000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 5,
            PublishUserId = true
        };
        string roomName = "Room" + randomRoomId;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Sala creada: " + roomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameOnline");
    }
}
