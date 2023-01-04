using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlockSwapEnabled : MonoBehaviour
{
    [SerializeField] private int number;
    [SerializeField] private GameObject[] firstObjects;
    [SerializeField] private GameObject[] SecondObjects;

    private AudioManager audioManager;

    public bool isFirstActive;

    public bool isBlockOn;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        isFirstActive = true;
        isBlockOn = false;
        for (int i = 0; i < firstObjects.Length; i++)
        {
            firstObjects[i].SetActive(true);
        }
        for (int i = 0; i < SecondObjects.Length; i++)
        {
            SecondObjects[i].SetActive(false);
        }

        Pathfinding.UpdateBlocks();


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isBlockOn = true;

            if (other.gameObject.GetComponent<Player>().currentNumber == number)
            {
                Debug.Log("Same number");
                if (isFirstActive)
                {
                    for (int i = 0; i < firstObjects.Length; i++)
                    {
                        firstObjects[i].SetActive(false);
                    }
                    for (int i = 0; i < SecondObjects.Length; i++)
                    {
                        SecondObjects[i].SetActive(true);
                    }

                    isFirstActive = false;
                }

                else
                {
                    for (int i = 0; i < firstObjects.Length; i++)
                    {
                        firstObjects[i].SetActive(true);
                    }
                    for (int i = 0; i < SecondObjects.Length; i++)
                    {
                        SecondObjects[i].SetActive(false);
                    }

                    isFirstActive = true;
                }

                Pathfinding.UpdateBlocks();
                audioManager.PlayRollSound();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isBlockOn = false;       
        }
    }
}
