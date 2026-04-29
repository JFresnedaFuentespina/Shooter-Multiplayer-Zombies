using Photon.Pun;
using UnityEngine;

public class ZombieLife : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health = 100f;
    public bool isAlive = true;
    public Animator animator;
    public GameManager gameManager;
    public PhotonView photonView;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC("TakeDamageViewId", RpcTarget.All, damage, photonView.ViewID);
    }

    [PunRPC]
    public void TakeDamageViewId(float damage, int viewId)
    {
        if (photonView.ViewID == viewId)
        {
            if (!isAlive) return;

            health -= damage;
            if (health <= 0f)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        Destroy(gameObject, 2.6f);
        
        if (!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            gameManager.enemiesKilled++;
        }

    }
}
