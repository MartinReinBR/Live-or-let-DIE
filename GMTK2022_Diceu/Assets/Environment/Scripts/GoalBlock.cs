using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalBlock : MonoBehaviour
{
    [SerializeField] private int number;
    [SerializeField] private int IndexOfNextLevel;

    private AudioManager audioManager;

    private bool isSwapingLevel;

    // Start is called before the first frame update
    void Start()
    {
        isSwapingLevel = false;
        audioManager = GetComponent<AudioManager>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().currentNumber == number)
            {
                Debug.Log("Same number");
                if (!isSwapingLevel)
                {
                    Debug.Log("DAB");
                    StartCoroutine(WaitForNextLevel());
                }
            }

            else
            {
                Debug.Log("Not Same");
            }
        }
    }

    IEnumerator WaitForNextLevel()
    {
        isSwapingLevel = true;
        audioManager.PlayRollSound();

        //yield on a new YieldInstruction that waits for 2 seconds.
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(IndexOfNextLevel);
    }

}
