using System;
using UnityEngine;

public class PolarisPhysics : MonoBehaviour
{
    // mass in kg
    public Rigidbody auvBody; 
    public MeshCollider auvCollider;
    public Transform auvTransform;

    private BuoyancyPassive buoyancyForces;
    private DragPassive dragForces;

    // Start is called before the first frame update
    void Start()
    {
        this.buoyancyForces = new BuoyancyPassive(
           this.auvBody,
           this.auvCollider,
           60f * 9.81f,
           100
        );

        this.dragForces = new DragPassive(auvBody, 1, 1, 1, 0.5f, 0.5f, 0.5f, 977f);
        this.auvBody.mass = 50;

    }

    // Update is called once per frame
    void Update()
    {
        
        // this.buoyancyForces.ApplyBuoyancyForce(0);
    }

    void FixedUpdate() {
        if (this.buoyancyForces.ApplyBuoyancyForce(0)) {
            // apply water drag forces
            this.dragForces.ApplyDragForces();
        }
    }
}

