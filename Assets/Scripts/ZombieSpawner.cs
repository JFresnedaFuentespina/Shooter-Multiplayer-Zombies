using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject zombiePrefab;

    public void SpawnZombie()
    {
        Instantiate(zombiePrefab, transform.position, transform.rotation);
    }
}
