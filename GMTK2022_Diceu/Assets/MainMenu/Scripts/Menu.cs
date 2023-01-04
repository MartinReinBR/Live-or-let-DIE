using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private AudioClip clickSounds;
    [SerializeField] private AudioClip hoverSounds;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnStartButton()
    {
        gameObject.GetComponent<Button>();
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSounds);
    }
    public void PlayHoverSound()
    {

    }
}
