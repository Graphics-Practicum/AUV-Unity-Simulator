using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        int lightRot = (Input.GetKey(KeyCode.LeftBracket) ? 1 : 0) - (Input.GetKey(KeyCode.RightBracket) ? 1 : 0);
        this.transform.RotateAround(transform.position, new Vector3(1f, 0, 0), lightRot * 0.5f);
    }
}
