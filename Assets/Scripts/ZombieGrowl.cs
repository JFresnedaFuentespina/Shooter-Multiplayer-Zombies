using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGrowl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;
    public ZombieLife zombieLife;
    public List<AudioClip> growlClips;
    public float growlInterval = 5f;
    void Start()
    {
        StartCoroutine(GrowlRoutine());
    }

    private IEnumerator GrowlRoutine()
    {
        while (zombieLife.isAlive)
        {
            yield return new WaitForSeconds(growlInterval);
            PlayRandomGrowl();
        }
    }

    public void PlayRandomGrowl()
    {
        if (growlClips.Count == 0) return;

        int index = Random.Range(0, growlClips.Count);
        audioSource.clip = growlClips[index];
        audioSource.Play();
    }
}
