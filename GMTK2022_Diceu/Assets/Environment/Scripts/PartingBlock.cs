using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartingBlock : MonoBehaviour
{
    [SerializeField] private GameObject groundToSetTag;

    private AudioManager audioManager;

    [SerializeField] private GameObject leftPart;
    [SerializeField] private GameObject rightPart;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }


    public void PartTheBlocks()
    {
        audioManager.PlayRollSound();
        StartCoroutine(leftPart.GetComponent<MoveDice>().PushCube(transform.position + (transform.TransformDirection(Vector3.left / 1.32f))));
        StartCoroutine(rightPart.GetComponent<MoveDice>().PushCube(transform.position + (transform.TransformDirection(Vector3.right) / 1.32f)));

        groundToSetTag.tag = "Ground";
        Pathfinding.UpdateBlocks();

    }

}
