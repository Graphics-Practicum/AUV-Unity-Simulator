using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    [SerializeField]
    private Torpedoes torpedo;

    [SerializeField]
    private string targetTag;
    private bool fire;

    void Start()
    {
        torpedo.SetTargetTag(targetTag);
    }

    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            fire = true;
        }
        if(Input.GetKeyUp("space"))
        {
            fire = false;
        }

        if(fire)
        {
            torpedo.Fire();
        }
    }
}
