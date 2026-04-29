using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator animator;
    public Transform playerTransform;
    public NavMeshAgent agent;
    public ZombieLife zombieLife;
    public float moveSpeed = 5f;
    public float attackRange = 1.5f;
    private enum STATE { IDLE, PURSUE, ATTACK, DIE }
    private STATE currentState = STATE.IDLE;
    public bool alreadyDead = false;
    private List<GameObject> playersInScene;

    void Start()
    {
        playersInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

        zombieLife = GetComponent<ZombieLife>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) return;

        if (!zombieLife.isAlive && !alreadyDead)
        {
            Stop();
            animator.SetTrigger("death");
            currentState = STATE.DIE;
            alreadyDead = true;
            return;
        }
        Process();
    }

    private void Process()
    {
        if (currentState == STATE.DIE) return;
        switch (currentState)
        {
            case STATE.IDLE:
                Idle();
                break;
            case STATE.PURSUE:
                Pursue();
                break;
            case STATE.ATTACK:
                Attack();
                break;
        }
    }

    void Idle()
    {
        currentState = STATE.PURSUE;
    }

    void Pursue()
    {
        FindClosestPlayer();
        animator.SetBool("run", true);
        agent.speed = moveSpeed;
        agent.isStopped = false;
        if (Vector3.Distance(agent.destination, playerTransform.position) > 0.5f)
        {
            agent.SetDestination(playerTransform.position);
        }

        if (CanAttack())
        {
            currentState = STATE.ATTACK;
        }
    }

    void Attack()
    {
        animator.SetBool("run", false);
        agent.isStopped = true;
        animator.SetTrigger("attack");
        if (!CanAttack())
        {
            currentState = STATE.PURSUE;
        }
    }

    bool CanAttack()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        return distance <= attackRange;
    }

    public void Stop()
    {
        animator.SetBool("run", false);
        agent.isStopped = true;
    }

    private void FindClosestPlayer()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;
        foreach (GameObject player in playersInScene)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }
        playerTransform = closestPlayer.transform;
    }
}
