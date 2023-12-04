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
    private static float AUV_MASS = 1; // mass in kg
    private static float AUV_BUOYANCY = 1.2f * 9.81f; // buoyancy in kg
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
        this.controlHelm = new ControlHelm(auvBody,
            new PositionLoop(
                loop_x: new PIDLateral(
                    kp: 0,
                    ki: 0,
                    kd: 0,
                    max_i: 100
                ),
                loop_y: new PIDLateral(
                    kp: 0,
                    ki: 0.5f,
                    kd: 1f,
                    max_i: 100
                ),
                loop_z: new PIDLateral(
                    kp: 0,
                    ki: 0,
                    kd: 0,
                    max_i: 100
                ),
                clampVal: 300
            ),
            new RotationLoop(
                loop_x: new PIDRotation(
                    kp: 0f,
                    ki: 0,
                    kd: 0,
                    max_i: 180
                ),
                loop_y: new PIDRotation(
                    kp: 0.001f,
                    ki: 0,
                    kd: 0,
                    max_i: 180
                ),
                loop_z: new PIDRotation(
                    kp: 0f,
                    ki: 0,
                    kd: 0,
                    max_i: 180
                ),
                clampVal: 20
            )
        );
    }

    // Update is called once per frame
    void Update()
    {
        float y_force = -PolarisPhysics.AUV_MASS * 9.81f + this.buoyancyForces.GetBuoyancyForce();
        this.controlHelm.ParseKeyControl(new Vector3(0, y_force, 0));
    }

    void FixedUpdate()
    {
        if (this.buoyancyForces.ApplyBuoyancyForce(0))
        {
            // apply water drag forces
            this.dragForces.ApplyDragForces();
        }

        this.auvBody.AddRelativeForce(this.controlHelm.GetLateralThrust());
        this.auvBody.AddRelativeTorque(this.controlHelm.GetRotationalThrust());
    }
}

