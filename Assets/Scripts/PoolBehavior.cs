using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehavior : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] noWalls;

    public bool[] test;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePool(test);
    }

    // Update is called once per frame
    void UpdatePool(bool[] status)
    {
        for(int i = 0; i<status.Length; i++)
        {
            noWalls[i].SetActive(status[i]);
            walls[i].SetActive(status[i]);
        }
        
    }
}
