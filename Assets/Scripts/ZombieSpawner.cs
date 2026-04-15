using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject zombiePrefab;
    public float spawnInterval = 5f;
    private float timer = 0f;
    public int maxZombies = 1;
    public int currentZombies = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentZombies < maxZombies)
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    private void SpawnZombie()
    {
        Instantiate(zombiePrefab, transform.position, transform.rotation);
        currentZombies++;
    }
}
