using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private bool shouldRemoveTagOnStart;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldRemoveTagOnStart)
        {
            gameObject.tag = "Untagged";
            Pathfinding.UpdateBlocks();
        } 
    }



}
