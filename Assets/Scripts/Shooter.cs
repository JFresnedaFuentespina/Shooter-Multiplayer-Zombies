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
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootSound;
        muzzleFlashParticleSystem = muzzleFlash.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("Hit: " + hit.transform.name);
            ZombieLife zombie = hit.transform.GetComponentInParent<ZombieLife>();

            if (zombie != null)
            {
                zombie.TakeDamage(50f);
            }
        }
    }
}
