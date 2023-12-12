using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehavior : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] noWalls;

    public GameObject[] shaderWalls;
    private bool[] status;
    public void UpdatePool(bool[] status)
    {
        this.status = status;
        for (int i = 0; i < status.Length; i++)
        {
            noWalls[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
            shaderWalls[i].SetActive(status[i]);
        }

    }
    public void Update()
    {
        for (int i = 0; i < status.Length; i++)
        {
            shaderWalls[i].SetActive(!status[i]);
        }
    }
}
