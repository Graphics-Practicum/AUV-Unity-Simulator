using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Whirlpool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        PolarisPhysics p = other.GetComponentInParent<PolarisPhysics>();
        p.kill(true);
    }
    // void OnTriggerExit(Collider other)
    // {
    //     PolarisPhysics p = other.GetComponentInParent<PolarisPhysics>();
    //     p.kill(false);
    // }
    void OnTriggerStay(Collider other)
    {
        Transform otherPos = other.attachedRigidbody.GetComponentInParent<Transform>();
        Transform thisPos = GetComponentInParent<Transform>();
        float scale = 500f * (otherPos.position.y - thisPos.position.y);
        float alpha = 0.5f;
        other.attachedRigidbody.AddForce(new Vector3(Math.Max(scale, 60) * (alpha * (thisPos.position.z - otherPos.position.z) + (1 - alpha) * (thisPos.position.x - otherPos.position.x)), -scale - (250 / scale) * (250 / scale), Math.Max(scale, 10) * (alpha * (otherPos.position.x - thisPos.position.x) + (1 - alpha) * (thisPos.position.z - otherPos.position.z))));
    }
}
