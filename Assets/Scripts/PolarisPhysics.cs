using System;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

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
    private static float AUV_MASS = 50; // mass in kg
    private static float AUV_BUOYANCY = 60f * 9.81f; // buoyancy in kg

    // Start is called before the first frame update
    void Start()
    {

        this.buoyancyForces = new BuoyancyPassive(
           this.auvBody,
           this.auvCollider,
           PolarisPhysics.AUV_BUOYANCY,
           300
        );
        this.auvBody.mass = PolarisPhysics.AUV_MASS;

        this.dragForces = new DragPassive(auvBody, 1, 1, 1, 0.5f, 0.5f, 0.5f, 977f);
        this.controlHelm = new ControlHelm(auvBody,
            new PositionLoop(
                loop_x: new PIDLateral(
                    kp: 5,
                    ki: 0,
                    kd: 50f,
                    max_i: 100
                ),
                loop_y: new PIDLateral(
                    kp: 100,
                    ki: 0,
                    kd: 20,
                    max_i: 100
                ),
                loop_z: new PIDLateral(
                    kp: 5,
                    ki: 0,
                    kd: 50,
                    max_i: 100
                ),
                clampVal: 300
            ),
            new RotationLoop(
                loop_x: new PIDRotation(
                    kp: 2f,
                    ki: 0,
                    kd: 0,
                    max_i: 180
                ),
                loop_y: new PIDRotation(
                    kp: 1.5f,
                    ki: 0,
                    kd: 10,
                    max_i: 180
                ),
                loop_z: new PIDRotation(
                    kp: 2f,
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
        this.controlHelm.ParseKeyControl(new Vector3(0, 0, 0));
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

