using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlockPartingDoor : MonoBehaviour
{
    [SerializeField] private int number;
    [SerializeField] private GameObject objectToMove;

    [SerializeField] private bool isDoorOpened;

    

    // Start is called before the first frame update
    void Start()
    {
        isDoorOpened = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().currentNumber == number)
            {
                Debug.Log("Same number");
                if (!isDoorOpened)
                {
                    objectToMove.GetComponent<PartingBlock>().PartTheBlocks();
                    isDoorOpened = true;
                }
            }

            else
            {
                Debug.Log("Not Same");
            }
        }
    }
}
