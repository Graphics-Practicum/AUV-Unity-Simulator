using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehavior : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] noWalls;

    public void UpdatePool(bool[] status)
    {
        for(int i = 0; i<status.Length; i++)
        {
            noWalls[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
        
    }
}
