using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarisPanCamera : MonoBehaviour
{

    public Transform lookAt;
    public float distanceFromTarget, lastMouseY;
    private Vector3 lastMousePosition = Vector3.zero, cumulativeRotations = Vector3.zero;
    private bool lockCamera;

    // Start is called before the first frame update
    void Start()
    {
        this.distanceFromTarget = -2f;
        this.lastMouseY = Input.mouseScrollDelta.y;
        this.lockCamera = false;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.L))
        {
            this.lockCamera = true;
        }
        if (Input.GetKey(KeyCode.O))
        {
            this.lockCamera = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = (Input.mousePosition - this.lastMousePosition);
            this.cumulativeRotations += delta;
            this.cumulativeRotations = new Vector3(
                this.NormalizeAngle(this.cumulativeRotations.x),
                this.NormalizeAngle(this.cumulativeRotations.y),
                this.NormalizeAngle(this.cumulativeRotations.z)
            );
            lastMousePosition = Input.mousePosition;
        }
        float currentMouseY = Input.mouseScrollDelta.y;
        this.distanceFromTarget += currentMouseY;
        this.lastMouseY = currentMouseY;

        // set camera position
        float delta_x = 0;
        float delta_y = 0;
        float delta_z = 0;

        // rotation about the y axis
        delta_y += Mathf.Sin(Mathf.Deg2Rad * this.cumulativeRotations.y) * this.distanceFromTarget;

        if (this.lockCamera)
        {
            Vector3 targetOrientation = this.lookAt.eulerAngles;
            delta_x += Mathf.Cos(Mathf.Deg2Rad * -targetOrientation.y) * this.distanceFromTarget;
            delta_z += Mathf.Sin(Mathf.Deg2Rad * -targetOrientation.y) * this.distanceFromTarget;

        }
        else
        {
            delta_x += Mathf.Cos(Mathf.Deg2Rad * this.cumulativeRotations.x) * this.distanceFromTarget;
            delta_z += Mathf.Sin(Mathf.Deg2Rad * this.cumulativeRotations.x) * this.distanceFromTarget;
        }


        Vector3 globalPosition = this.lookAt.position;
        Vector3 cameraGlobalPosition = new Vector3(globalPosition.x + delta_x, globalPosition.y + delta_y, globalPosition.z + delta_z);

        this.transform.position = cameraGlobalPosition;
        this.transform.LookAt(this.lookAt);
    }
    float NormalizeAngle(float angle)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        return angle;
    }
}
