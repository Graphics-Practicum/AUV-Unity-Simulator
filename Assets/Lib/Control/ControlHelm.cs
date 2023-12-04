using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class ControlHelm
{


    private Rigidbody rigidbody;
    private PositionLoop positionLoop;
    private RotationLoop rotationLoop;

    private bool is_positional_control = true;
    private Vector3 position_desire, orientation_desire;
    private Vector3 lateral_thrust_output = Vector3.zero, rotational_thrust_output = Vector3.zero;

    public ControlHelm(
        Rigidbody rigidBody,
        PositionLoop positionLoop,
        RotationLoop rotationLoop
    )
    {
        this.rigidbody = rigidBody;
        this.positionLoop = positionLoop;
        this.rotationLoop = rotationLoop;

        this.orientation_desire = rigidBody.transform.localEulerAngles;
        this.position_desire = rigidBody.transform.localPosition;
    }

    // https://docs.unity3d.com/ScriptReference/Event-keyCode.html
    public void ParseKeyControl(Vector3 passive_lateral_forces)
    {
        // Process WASD (forward backward left right)
        // Q = depth up
        // E = depth down
        if (this.is_positional_control)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                this.position_desire.x -= 1;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                this.position_desire.z += 1;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.position_desire.x -= 1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                this.position_desire.z += 1;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.position_desire.y += 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.position_desire.y -= 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.orientation_desire.y -= 5;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.orientation_desire.y += 5;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.orientation_desire = this.rigidbody.transform.localEulerAngles;
            this.orientation_desire.x = 0;
            this.orientation_desire.z = 0;
            this.position_desire = this.rigidbody.transform.localPosition;
        }

        Vector3 real_position = this.rigidbody.transform.localPosition;
        Vector3 thrusts = this.positionLoop.Step(real_position, this.position_desire, passive_lateral_forces);
        this.lateral_thrust_output = thrusts;

        Vector3 real_rotation = this.rigidbody.transform.localEulerAngles;
        Vector3 rotation_thrusts = this.rotationLoop.Step(real_rotation, this.orientation_desire, Vector3.zero);
        Debug.Log("Current: " + real_position + " Desire: " + this.position_desire + " Output: " + thrusts);
        // Debug.Log("Current: " + real_rotation + " Desire: " + this.orientation_desire + " Output: " + rotation_thrusts);
        this.rotational_thrust_output = rotation_thrusts; //(this.rigidbody.rotation * Quaternion.Euler(rotation_thrusts)).eulerAngles;
    }

    public Vector3 GetLateralThrust()
    {
        return this.lateral_thrust_output;
    }
    public Vector3 GetRotationalThrust()
    {
        return this.rotational_thrust_output;
    }
}
