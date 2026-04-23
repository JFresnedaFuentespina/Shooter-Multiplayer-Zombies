using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int enemiesToSpawn;
    public int enemiesSpawned = 0;
    public int enemiesKilled = 0;
    public int round = 0;
    public float spawnDelay = 2f;
    public List<GameObject> zombieSpawns;
    public TextMeshProUGUI roundText;
    public PhotonView photonView;
    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            if (enemiesSpawned - enemiesKilled <= 0)
            {
                StartRound();
            }
        }
    }



    public void StartRound()
    {
        round++;
        enemiesToSpawn = round * 5;
        if (PhotonNetwork.InRoom)
        {
            Hashtable hash = new Hashtable();
            hash.Add("currentRound", round);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        else
        {
            DisplayNextRound(round);
        }
        StartCoroutine(SpawnZombiesWithDelay(enemiesToSpawn));
    }

    private void DisplayNextRound(int round)
    {
        roundText.text = "Round: " + round;
    }

    private IEnumerator SpawnZombiesWithDelay(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnDelay);
        }
        yield return null;
    }

    public void SpawnZombie()
    {
        if (zombieSpawns.Count == 0) return;
        int spawnIndex = Random.Range(0, zombieSpawns.Count);
        zombieSpawns[spawnIndex].GetComponent<ZombieSpawner>().SpawnZombie(zombieSpawns[spawnIndex].transform.position);
        enemiesSpawned++;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (photonView.IsMine)
        {
            if (changedProps["currentRound"] != null)
            {
                DisplayNextRound((int)changedProps["currentRound"]);
            }
        }
    }
}
