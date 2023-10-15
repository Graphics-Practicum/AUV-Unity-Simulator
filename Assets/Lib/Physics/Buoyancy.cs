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
    
    public BuoyancyPassive(
            Rigidbody body, 
            MeshCollider collider, 
            float buoyancyForce,
            int buoyancyFidelity = 2000 
        ) {
        this.body = body;
        this.collider = collider;
        this.buoyancyForce = buoyancyForce;


        Assert.IsTrue(buoyancyFidelity <= this.collider.sharedMesh.vertices.Length);
        System.Random rnd = new System.Random(0);
        // apply water drag forces
        this.objVertices = this.collider.sharedMesh.vertices.OrderBy(x => rnd.Next()).Take(buoyancyFidelity).ToArray();
        this.transformedVertices = (Vector3[]) this.objVertices.Clone();
    }

    public bool ApplyBuoyancyForce(float waterLevel) {
        bool touchedWater = false;

        Transform transform = this.collider.transform;
        transform.TransformPoints(this.objVertices, this.transformedVertices);

        for(int i = 0; i < this.objVertices.Length; i++) {
            if(this.transformedVertices[i].y <= waterLevel) {
                Vector3 direction = new Vector3(0, this.buoyancyForce / (float) this.objVertices.Length, 0);
                this.body.AddForceAtPosition(direction, this.transformedVertices[i]);
                touchedWater |= true;
            }
        }
        return touchedWater;
    }
}
