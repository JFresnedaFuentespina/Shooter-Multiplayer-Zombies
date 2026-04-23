using UnityEngine;

public class PlayersSpawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
