using Photon.Pun;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public void SpawnZombie(Vector3 position)
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Instantiate("Zombie", position, Quaternion.identity);
        }
        else
        {
            Instantiate(Resources.Load("Zombie"), position, Quaternion.identity);
        }
    }
}
