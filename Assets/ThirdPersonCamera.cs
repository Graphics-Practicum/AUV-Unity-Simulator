using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform sub_transform;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = sub_transform.rotation;
        transform.position = sub_transform.position - sub_transform.forward * 5 + new Vector3(0, 1.5f, 0);
    }
}
