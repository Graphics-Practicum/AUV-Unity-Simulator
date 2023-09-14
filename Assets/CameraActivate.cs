using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActivate : MonoBehaviour
{
    public int cameraID;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");
        GameObject sub = GameObject.Find("Sub");
        Debug.Log(sub.GetComponent<Sub>().activeCam);
        if (cameraID == sub.GetComponent<Sub>().activeCam)
        {
            this.GetComponent<Camera>().enabled = true;
        }
        else
        {
            this.GetComponent<Camera>().enabled = false;
        }
    }
}
