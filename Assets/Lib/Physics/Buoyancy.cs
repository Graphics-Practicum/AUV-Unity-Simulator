using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;


/* Attach this script to any RigidBody you want to apply water physics to. This script will
add the following functionalities:
    1. water bouyancy
    2. water positional drag
    3. water angular drag
*/

/// <summary>
/// Calculates all possive forces acting on an gameObject in water.
/// </summary>
public class BuoyancyPassive
{
    private Rigidbody body;
    private MeshCollider collider;
    private float buoyancyForce;
    private Vector3[] objVertices;
    private Vector3[] transformedVertices;
    private float total_passive_force;

    public BuoyancyPassive(
            Rigidbody body,
            MeshCollider collider,
            float buoyancyForce,
            int buoyancyFidelity = 2000
        )
    {
        this.body = body;
        this.collider = collider;
        this.buoyancyForce = buoyancyForce;
        this.total_passive_force = 0;


        Assert.IsTrue(buoyancyFidelity <= this.collider.sharedMesh.vertices.Length);
        System.Random rnd = new System.Random(0);
        // apply water drag forces
        this.objVertices = this.collider.sharedMesh.vertices.OrderBy(x => rnd.Next()).Take(buoyancyFidelity).ToArray();
        this.transformedVertices = (Vector3[])this.objVertices.Clone();
    }

    public bool ApplyBuoyancyForce(float waterLevel)
    {
        bool touchedWater = false;

        Transform transform = this.collider.transform;
        transform.TransformPoints(this.objVertices, this.transformedVertices);

        this.total_passive_force = 0;
        for (int i = 0; i < this.objVertices.Length; i++)
        {
            if (this.transformedVertices[i].y <= waterLevel)
            {
                float little_force = this.buoyancyForce / (float)this.objVertices.Length;
                Vector3 direction = new Vector3(0, little_force, 0);
                this.total_passive_force += little_force;
                this.body.AddForceAtPosition(direction, this.transformedVertices[i]);
                touchedWater |= true;
            }
        }
        return touchedWater;
    }

    public float GetBuoyancyForce()
    {
        return this.total_passive_force;
    }
}
