using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int enemiesToSpawn;
    public int enemiesSpawned = 0;
    public int enemiesKilled = 0;
    public int round = 0;
    public float spawnDelay = 2f;
    public List<GameObject> zombieSpawns;
    public TextMeshProUGUI roundText;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (enemiesSpawned - enemiesKilled <= 0)
        {
            StartRound();
        }
    }



    public void StartRound()
    {
        round++;
        enemiesToSpawn = round * 5;
        roundText.text = "Round: " + round;
        StartCoroutine(SpawnZombiesWithDelay(enemiesToSpawn));
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
        zombieSpawns[spawnIndex].GetComponent<ZombieSpawner>().SpawnZombie();
        enemiesSpawned++;
    }
}
