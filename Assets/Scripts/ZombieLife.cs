using UnityEngine;

public class ZombieLife : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health = 100f;
    public bool isAlive = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive) return;

        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        GetComponent<ZombieBehaviour>().enabled = false;
        Destroy(gameObject, 5f);
    }
}
