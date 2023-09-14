using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sub : MonoBehaviour
{
    // Update is called once per frame
    public int activeCam = 0;
    void Update()
    {
        int strafe = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        int vert = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        int forward = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        int sway = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        transform.position = transform.position + transform.forward * 0.1f * forward + transform.right * 0.1f * strafe + new Vector3(0, vert * 0.1f, 0);
        transform.RotateAround(transform.position, new Vector3(0, 0.1f, 0), sway);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab pressed");
            Debug.Log(activeCam);
        }
        activeCam = (activeCam + (Input.GetKeyDown(KeyCode.Tab) ? 1 : 0)) % 3;
    }
}
