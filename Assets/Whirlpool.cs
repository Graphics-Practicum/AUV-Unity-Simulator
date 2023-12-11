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

    void OnTriggerStay(Collider other)
    {
        Transform otherPos = other.attachedRigidbody.GetComponentInParent<Transform>();
        Transform thisPos = GetComponentInParent<Transform>();
        float scale = 10f * (otherPos.position.y - thisPos.position.y);
        float alpha = 0.5f;
        other.attachedRigidbody.AddForce(new Vector3(Math.Max(scale, 10) * (alpha * (thisPos.position.z - otherPos.position.z) + (1 - alpha) * (thisPos.position.x - otherPos.position.x)), -scale / 10 - (1000 / scale) * (1000 / scale), Math.Max(scale, 10) * (alpha * (otherPos.position.x - thisPos.position.x) + (1 - alpha) * (thisPos.position.z - otherPos.position.z))));
    }
}
