using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] rollOverSounds;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSound()
    {
        audioSource.PlayOneShot(rollOverSounds[(int)Mathf.Floor(Random.Range(0, rollOverSounds.Length))]);
    }

}
