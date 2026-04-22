using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button multiplayerButton;
    void Start()
    {
        Debug.Log("Conectando al servidor...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectando a un lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby!");
        multiplayerButton.interactable = true;
    }
}
