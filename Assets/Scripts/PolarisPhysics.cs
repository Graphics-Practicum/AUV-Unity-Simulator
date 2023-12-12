using System;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using System.Collections;


public class PolarisPhysics : MonoBehaviour
{
    // mass in kg
    public Rigidbody auvBody;
    public MeshCollider auvCollider;
    public Transform auvTransform;

    private BuoyancyPassive buoyancyForces;
    private DragPassive dragForces;
    private ControlHelm controlHelm;

    /** set this to whatever **/
    private static float AUV_MASS = 100; // mass in kg
    private static float AUV_BUOYANCY = 130f * 9.81f; // buoyancy in kg
    // Start is called before the first frame update
    void Start()
    {
        this.auvBody.centerOfMass = new Vector3(0, -0.3f, 0);
        this.buoyancyForces = new BuoyancyPassive(
           this.auvBody,
           this.auvCollider,
           PolarisPhysics.AUV_BUOYANCY,
           1000
        );
        this.auvBody.mass = PolarisPhysics.AUV_MASS;

        this.dragForces = new DragPassive(auvBody, 1, 1, 1, 0.5f, 0.5f, 0.5f, 977f);
        this.controlHelm = new ControlHelm(auvBody, new PIDLateral(
            0.1f,
            0.0f,
            0,
            10
        ));
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void toggle_soft()
    {
        this.controlHelm.toggle_soft();
    }
    public void kill(bool killed)
    {
        if (killed)
        {
            controlHelm.kill();
        }
        else
        {
            controlHelm.unkill();
        }
    }
    void FixedUpdate()
    {
        if (this.buoyancyForces.ApplyBuoyancyForce(9.7f))
        {
            // apply water drag forces
            this.dragForces.ApplyDragForces();
        }

        // float y_force = -PolarisPhysics.AUV_MASS * 9.81f + this.buoyancyForces.GetBuoyancyForce();
        this.controlHelm.ParseKeyControl();

        // Debug.Log(this.controlHelm.GetLateralThrust());
        // this.auvBody.velocity += this.controlHelm.GetLateralThrust();
        // this.auvBody.angularVelocity += this.controlHelm.GetRotationalThrust();
        // // this.auvBody.AddRelativeForce(this.controlHelm.GetLateralThrust());
        // this.auvBody.AddRelativeTorque(this.controlHelm.GetRotationalThrust());
    }
}

