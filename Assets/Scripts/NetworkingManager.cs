using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public Button play;
    public Button exit;
    public Button createRoom;

    public GameObject roomListContainer;
    public GameObject roomsPanel;
    public GameObject roomInfoPrefab;
    public TMP_InputField roomInputField;
    public GameObject roomNameText;

    private Dictionary<string, RoomInfo> rooms = new Dictionary<string, RoomInfo>();

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        play.onClick.AddListener(StartGame);
        exit.onClick.AddListener(Exit);
        createRoom.onClick.AddListener(MakeRoom);

        play.interactable = false;

        if (PhotonNetwork.IsConnected)
            StartCoroutine(WaitForJoinLobby());

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    IEnumerator WaitForJoinLobby()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
            yield return null;
    }

    void Exit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby");
    }

    void MakeRoom()
    {
        string roomName = roomInputField.text;

        if (string.IsNullOrWhiteSpace(roomName))
            roomName = "Room_" + Random.Range(1000, 9999);

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 5,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.CreateRoom(roomName, options);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Rooms recibidas: " + roomList.Count);
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
                rooms.Remove(room.Name);
            else
                rooms[room.Name] = room;
        }

        UpdateRoomsUI();
    }

    void UpdateRoomsUI()
    {
        foreach (Transform child in roomsPanel.transform)
            Destroy(child.gameObject);

        foreach (RoomInfo room in rooms.Values)
        {
            GameObject item = Instantiate(roomInfoPrefab, roomsPanel.transform);

            item.transform.Find("RoomName")
                .GetComponent<TextMeshProUGUI>()
                .text = room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")";

            string roomName = room.Name;

            item.transform.Find("Join")
                .GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    PhotonNetwork.JoinRoom(roomName);
                });
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            play.interactable = true;
        }

        roomListContainer.SetActive(false);
        roomNameText.SetActive(true);
        roomNameText.GetComponent<TextMeshProUGUI>().text = "Sala: " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Entraste en: " + PhotonNetwork.CurrentRoom.Name);
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("GameOnline");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby();
    }
}