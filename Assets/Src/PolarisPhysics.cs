using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarisPhysics : MonoBehaviour
{
    // mass in kg
    public Rigidbody auvBody; 
    public float mass = 30.58F;

    // Start is called before the first frame update
    void Start()
    {
        auvBody.mass = mass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
