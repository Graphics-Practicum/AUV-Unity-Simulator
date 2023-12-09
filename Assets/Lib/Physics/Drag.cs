using System;
using UnityEngine;

/* 
    define YZ plane as "front-back" since X axis is forward/backward
    define XY plane as "left-right" since Z axis is left/right 
    define XZ as plane "top-down" since Y axis to top/down
*/

public class DragPassive
{
    private Rigidbody body;
    private float frontBackDragConstant,
        leftRightDragConstant,
        topDownDragConstant;

    public DragPassive(
        Rigidbody body,
        float frontBackCrossSectionalArea,
        float leftRightCrossSectionalArea,
        float topDownCrossSectionalArea,
        float frontBackDragCoeff,
        float leftRightDragCoeff,
        float topDownDragCoeff,
        float fluidDensity
    )
    {
        this.body = body;

        this.frontBackDragConstant = 0.5f * frontBackCrossSectionalArea * frontBackDragCoeff * fluidDensity;
        this.leftRightDragConstant = 0.5f * leftRightCrossSectionalArea * leftRightDragCoeff * fluidDensity;
        this.topDownDragConstant = 0.5f * topDownCrossSectionalArea * topDownDragCoeff * fluidDensity;

        this.body.angularDrag = 2f;
    }

    public void ApplyDragForces()
    {
        Vector3 velInWorldFrame = body.velocity;
        float x_vel = velInWorldFrame.x;
        float y_vel = velInWorldFrame.y;
        float z_vel = velInWorldFrame.z;

        float frontBackForce = -this.frontBackDragConstant * x_vel * x_vel * Math.Sign(x_vel);
        float topDownForce = -this.topDownDragConstant * y_vel * y_vel * Math.Sign(y_vel);
        float leftRightForce = -this.leftRightDragConstant * z_vel * z_vel * Math.Sign(z_vel);

        this.body.AddForce(new Vector3(frontBackForce, 0, 0));
        this.body.AddForce(new Vector3(0, topDownForce, 0));
        this.body.AddForce(new Vector3(0, 0, leftRightForce));
    }
}