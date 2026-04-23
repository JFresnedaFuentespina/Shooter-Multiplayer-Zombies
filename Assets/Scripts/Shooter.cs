using Photon.Pun;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerCamera;
    public GameObject muzzleFlash;
    private ParticleSystem muzzleFlashParticleSystem;
    private AudioSource audioSource;
    public AudioClip shootSound;
    public Animator animator;
    public float range = 100f;
    public PhotonView photonView;

    private GameOverManager gameOverManager;
    private PauseManager pauseManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootSound;
        muzzleFlashParticleSystem = muzzleFlash.GetComponent<ParticleSystem>();

        gameOverManager = FindAnyObjectByType<GameOverManager>();
        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // estar online y photonview no nos pertenece
        if (PhotonNetwork.InRoom && !photonView.IsMine) return;

        if (gameOverManager.isPaused) return;
        if (pauseManager.isPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        audioSource.Play();
        animator.SetTrigger("Shoot");

        muzzleFlashParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        muzzleFlashParticleSystem.Play();

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            ZombieLife zombie = hit.transform.GetComponentInParent<ZombieLife>();

            if (zombie != null)
            {
                zombie.TakeDamage(50f);
            }
        }
    }
}
