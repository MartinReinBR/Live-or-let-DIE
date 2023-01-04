using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] rollOverSounds;
    [SerializeField] private AudioClip drawFightSound;
    [SerializeField] private AudioClip winFightsound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSound()
    {
        audioSource.PlayOneShot(rollOverSounds[(int)Mathf.Floor(Random.Range(0, rollOverSounds.Length))]);
    }

    public void PlayDrawSound()
    {
        audioSource?.PlayOneShot(drawFightSound,0.7f);
    }

    public void PlayWinSound()
    {
        audioSource?.PlayOneShot(winFightsound,0.6f);
    }

}
