using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class ControlHelm
{


    private Rigidbody rigidbody;
    private PIDLateral heightLoop;
    private float height_desire = 0;
    public ControlHelm(
        Rigidbody rigidBody,
        PIDLateral heightLoop
    )
    {
        this.rigidbody = rigidBody;
        this.heightLoop = heightLoop;
    }

    // https://docs.unity3d.com/ScriptReference/Event-keyCode.html
    public void ParseKeyControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.rigidbody.AddRelativeForce(new Vector3(1, 0, 0) * 100);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.rigidbody.AddRelativeForce(new Vector3(0, 0, 1) * 100);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.rigidbody.AddRelativeForce(new Vector3(1, 0, 0) * -100);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.rigidbody.AddRelativeForce(new Vector3(0, 0, 1) * -100);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.rigidbody.AddRelativeTorque(new Vector3(0, 1, 0) * -10);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.rigidbody.AddRelativeTorque(new Vector3(0, 1, 0) * 10);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            this.height_desire -= 0.1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            this.height_desire += 0.1f;
        }

        float velocity = this.heightLoop.Step(this.rigidbody.position.y, this.height_desire);
        this.rigidbody.velocity += new Vector3(0, velocity, 0);

    }

    //     // Process WASD (forward backward left right)
    //     // Q = depth up
    //     // E = depth down
    //     if (this.is_positional_control)
    //     {
    //         if (Input.GetKeyDown(KeyCode.W))
    //         {
    //             this.position_desire.x -= 1;
    //         }
    //         if (Input.GetKeyDown(KeyCode.A))
    //         {
    //             this.position_desire.z -= 1;
    //         }
    //         if (Input.GetKeyDown(KeyCode.S))
    //         {
    //             this.position_desire.x += 1;
    //         }
    //         if (Input.GetKeyDown(KeyCode.D))
    //         {
    //             this.position_desire.z += 1;
    //         }
    //         if (Input.GetKeyDown(KeyCode.Q))
    //         {
    //             this.position_desire.y += 0.5f;
    //         }
    //         if (Input.GetKeyDown(KeyCode.E))
    //         {
    //             this.position_desire.y -= 0.5f;
    //         }
    //         if (Input.GetKeyDown(KeyCode.LeftArrow))
    //         {
    //             this.orientation_desire.y -= 5;
    //         }
    //         if (Input.GetKeyDown(KeyCode.RightArrow))
    //         {
    //             this.orientation_desire.y += 5;
    //         }
    //         if (Input.GetKeyDown(KeyCode.Space))
    //         {
    //             this.is_soft = true;
    //         }
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         this.orientation_desire = this.rigidbody.transform.localEulerAngles;
    //         this.orientation_desire.x = 0;
    //         this.orientation_desire.z = 0;
    //         this.position_desire = this.rigidbody.transform.localPosition;
    //     }

    //     Vector3 real_position = this.rigidbody.transform.localPosition;
    //     Vector3 thrusts = this.positionLoop.Step(real_position, this.position_desire, passive_lateral_forces);
    //     this.lateral_thrust_output = thrusts;

    //     Vector3 real_rotation = this.rigidbody.transform.localEulerAngles;
    //     Vector3 rotation_thrusts = this.rotationLoop.Step(real_rotation, this.orientation_desire, Vector3.zero);
    //     Debug.Log("Current: " + real_position + " Desire: " + this.position_desire + " Output: " + thrusts);
    //     // Debug.Log("Current: " + real_rotation + " Desire: " + this.orientation_desire + " Output: " + rotation_thrusts);
    //     this.rotational_thrust_output = rotation_thrusts; //(this.rigidbody.rotation * Quaternion.Euler(rotation_thrusts)).eulerAngles;
    // }

    // public Vector3 GetLateralThrust()
    // {
    //     if (this.is_soft) return Vector3.zero;
    //     return this.lateral_thrust_output;
    // }
    // public Vector3 GetRotationalThrust()
    // {
    //     if (this.is_soft) return Vector3.zero;
    //     return this.rotational_thrust_output;
    // }
}
